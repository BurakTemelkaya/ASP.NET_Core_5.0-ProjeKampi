using BusinessLayer.Abstract;
using CoreLayer.Utilities.FileUtilities;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class MessageDraftManager : IMessageDraftService
    {
        readonly IMessageDraftDal _messageDraftDal;
        readonly IBusinessUserService _businessUserService;
        public MessageDraftManager(IMessageDraftDal messageDraftDal, IBusinessUserService businessUserService)
        {
            _messageDraftDal = messageDraftDal;
            _businessUserService = businessUserService;
        }

        public async Task<int> GetCountAsync(Expression<Func<MessageDraft, bool>> filter = null)
        {
            return await _messageDraftDal.GetCountAsync(filter);
        }

        public async Task<List<MessageDraft>> GetListAsync(Expression<Func<MessageDraft, bool>> filter = null)
        {
            var values = await _messageDraftDal.GetListAllAsync(filter);
            foreach (var item in values)
                item.Details = await TextFileManager.ReadTextFileAsync(item.Details, 30);
            return values;
        }

        public async Task<List<MessageDraft>> GetMessageDraftListAsync(Expression<Func<MessageDraft, bool>> filter = null)
        {
            var values = await _messageDraftDal.GetMessageDraftListAsync(filter);
            foreach (var item in values)
                item.Details = await TextFileManager.ReadTextFileAsync(item.Details, 30);
            return values;
        }

        public async Task<List<MessageDraft>> GetMessageDraftListByUserNameAsync(string userName, Expression<Func<MessageDraft, bool>> filter = null)
        {
            var user = await _businessUserService.FindByUserNameAsync(userName);
            if (user != null)
            {
                var values = await _messageDraftDal.GetMessageDraftListByUserIdAsync(user.Id, filter);
                foreach (var item in values)
                    item.Details = await TextFileManager.ReadTextFileAsync(item.Details, 30);
                return values;
            }
            return null;
        }

        public async Task AddAsync(MessageDraft t, string userName)
        {
            var user = await _businessUserService.FindByUserNameAsync(userName);
            t.UserId = user.Id;
            t.Details = await TextFileManager.TextFileAddAsync(t.Details, TextFileManager.GetMessageContentFileLocation());
            await _messageDraftDal.InsertAsync(t);
        }

        public async Task DeleteAsync(int id, string userName)
        {
            var user = await _businessUserService.FindByUserNameAsync(userName);
            var value = await _messageDraftDal.GetByIDAsync(id);
            if (user.Id == value.UserId)
            {
                await _messageDraftDal.DeleteAsync(value);
            }
        }

        public async Task<MessageDraft> GetByFilterAsync(string userName, Expression<Func<MessageDraft, bool>> filter = null)
        {
            var user = await _businessUserService.FindByUserNameAsync(userName);
            var value = await _messageDraftDal.GetByFilterAsync(filter);
            if (user.Id == value.UserId)
            {
                value.Details = await TextFileManager.ReadTextFileAsync(value.Details);
                return value;
            }
            return null;
        }

        public async Task<MessageDraft> GetByIDAsync(int id, string userName)
        {
            var user = await _businessUserService.FindByUserNameAsync(userName);
            var value = await _messageDraftDal.GetByIDAsync(id);
            if (user.Id == value.UserId)
            {
                value.Details = await TextFileManager.ReadTextFileAsync(value.Details);
                return value;
            }
            return null;
        }

        public async Task UpdateAsync(MessageDraft t, string userName)
        {
            var user = await _businessUserService.FindByUserNameAsync(userName);
            var value = await _messageDraftDal.GetByIDAsync(t.MessageDraftID);
            if (user.Id == value.UserId)
                await _messageDraftDal.UpdateAsync(t);
        }
    }
}
