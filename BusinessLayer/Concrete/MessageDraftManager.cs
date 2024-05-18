using AutoMapper;
using BusinessLayer.Abstract;
using BusinessLayer.Constants;
using CoreLayer.Utilities.Business;
using CoreLayer.Utilities.FileUtilities;
using CoreLayer.Utilities.Results;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class MessageDraftManager : ManagerBase, IMessageDraftService
    {
        readonly IMessageDraftDal _messageDraftDal;
        readonly IUserBusinessService _businessUserService;
        public MessageDraftManager(IMessageDraftDal messageDraftDal, IUserBusinessService businessUserService, IMapper mapper) : base(mapper)
        {
            _messageDraftDal = messageDraftDal;
            _businessUserService = businessUserService;
        }

        public async Task<IDataResult<int>> GetCountAsync(Expression<Func<MessageDraft, bool>> filter = null)
        {
            return new SuccessDataResult<int>(await _messageDraftDal.GetCountAsync(filter));
        }

        public async Task<IDataResult<int>> GetCountByUserNameAsync(string userName)
        {
            var user = await _businessUserService.GetByUserNameAsync(userName);
            var count = await GetCountAsync(x => x.UserId == user.Data.Id);
            return new SuccessDataResult<int>(count.Data);
        }

        public async Task<IDataResult<List<MessageDraft>>> GetMessageDraftListByUserNameAsync(string userName, int length = 30)
        {
            var user = await _businessUserService.GetByUserNameAsync(userName);

            if (!user.Success)
            {
                return new ErrorDataResult<List<MessageDraft>>(user.Message);
            }

            var values = await _messageDraftDal.GetMessageDraftListByUserIdAsync(user.Data.Id);
            foreach (var item in values)
                item.Details = await TextFileManager.ReadTextFileAsync(item.Details, length);
            return new SuccessDataResult<List<MessageDraft>>(values);

        }


        public async Task<IResultObject> AddAsync(MessageDraft t, string userName)
        {
            var user = await _businessUserService.GetByUserNameAsync(userName);

            if (!user.Success)
            {
                return new ErrorResult(user.Message);
            }

            t.UserId = user.Data.Id;
            t.Details = await TextFileManager.TextFileAddAsync(t.Details, ContentFileLocations.GetMessageDraftContentFileLocation(), ContentFileLocations.GetMessageDraftContentFileLocation());
            await _messageDraftDal.InsertAsync(t);
            return new SuccessResult();
        }

        public async Task<IResultObject> DeleteAsync(int id, string userName)
        {

            var user = await _businessUserService.GetByUserNameAsync(userName);

            IResultObject result = BusinessRules.Run(UserNotEmpty(user), IdNotEqualZero(id));

            if (!result.Success)
            {
                return new ErrorResult(result.Message);
            }

            var value = await _messageDraftDal.GetMessageDraftByUserIdAsync(user.Data.Id, x => x.MessageDraftID == id);
            if (user.Data.Id == value.UserId)
            {
                await _messageDraftDal.DeleteAsync(value);
                DeleteFileManager.DeleteFile(value.Details);
                return new SuccessResult();
            }

            return new ErrorResult("Bu mesaj " + user.Data.UserName + " adlı kullanıcıya ait değil.");
        }

        public async Task<IResultObject> DeleteMessageDraftsAsync(List<string> ids, string userName)
        {
            IResultObject result = BusinessRules.Run(IdsNotNull(ids));

            if (!result.Success)
            {
                return result;
            }

            List<MessageDraft> messageDrafts = new();

            foreach (var id in ids)
            {
                try
                {
                    var message = await GetByIDAsync(Convert.ToInt32(id), userName);
                    messageDrafts.Add(message.Data);
                    message.Data.User = null;
                    DeleteFileManager.DeleteFile(message.Data.Details);
                }
                catch
                {
                    continue;
                }

            }

            if (messageDrafts.Count == 0)
            {
                return new ErrorResult(Messages.MessageDraftsNotDeleting);
            }

            await _messageDraftDal.DeleteRangeAsync(messageDrafts);
            return new SuccessResult();
        }

        public async Task<IDataResult<MessageDraft>> GetByIDAsync(int id, string userName)
        {
            var user = await _businessUserService.GetByUserNameAsync(userName);

            IResultObject result = BusinessRules.Run(IdNotEqualZero(id), UserNotEmpty(user));

            if (!result.Success)
            {
                return new ErrorDataResult<MessageDraft>(result.Message);
            }

            var value = await _messageDraftDal.GetMessageDraftByUserIdAsync(user.Data.Id, x => x.MessageDraftID == id);
            if (value != null && user.Data.Id == value.UserId)
            {
                value.Details = await TextFileManager.ReadTextFileAsync(value.Details);
                return new SuccessDataResult<MessageDraft>(value);
            }
            return null;
        }

        public async Task<IResultObject> UpdateAsync(MessageDraft t, string userName)
        {
            var user = await _businessUserService.GetByUserNameAsync(userName);

            IResultObject result = BusinessRules.Run(IdNotEqualZero(t.MessageDraftID), UserNotEmpty(user));

            if (!result.Success)
            {
                return result;
            }

            var value = await _messageDraftDal.GetMessageDraftByUserIdAsync(user.Data.Id, x => x.MessageDraftID == t.MessageDraftID);

            if (user.Data.Id == value.UserId)
            {
                if (await TextFileManager.ReadTextFileAsync(value.Details) != t.Details)
                {
                    DeleteFileManager.DeleteFile(t.Details);
                    t.Details = await TextFileManager.TextFileAddAsync(t.Details, ContentFileLocations.GetMessageDraftContentFileLocation(), ContentFileLocations.GetMessageDraftImageContentFileLocation());
                }
                else
                {
                    t.Details = value.Details;
                }
                t.UserId = user.Data.Id;
                await _messageDraftDal.UpdateAsync(t);


                return new SuccessResult();
            }

            return new ErrorResult(Messages.MessageDraftIsNotAuthors);
        }

        IResultObject IdNotEqualZero(int id)
        {
            if (id == 0)
            {
                return new ErrorResult(Messages.MessageDraftNotFound);
            }
            return new SuccessResult();
        }

        IResultObject IdsNotNull(List<string> ids)
        {
            if (ids == null)
            {
                return new ErrorResult(Messages.MessageDraftsNotFound);
            }
            return new SuccessResult();
        }
    }
}
