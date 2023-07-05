using AutoMapper;
using BusinessLayer.Abstract;
using BusinessLayer.Constants;
using BusinessLayer.ValidationRules;
using CoreLayer.Aspects.AutoFac.Caching;
using CoreLayer.Aspects.AutoFac.Validation;
using CoreLayer.Utilities.Business;
using CoreLayer.Utilities.FileUtilities;
using CoreLayer.Utilities.Results;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class MessageManager : ManagerBase, IMessageService
    {
        private readonly IMessageDal _messageDal;
        private readonly IBusinessUserService _userService;

        public MessageManager(IMessageDal message2Dal, IMapper mapper, IBusinessUserService userService) : base(mapper)
        {
            _messageDal = message2Dal;
            _userService = userService;
        }

        [CacheAspect]
        public async Task<IDataResult<List<Message>>> GetInboxWithMessageListAsync(string userName, string search = null, Expression<Func<Message, bool>> filter = null, int take = 0, int skip = 0)
        {
            var user = await _userService.GetByUserNameAsync(userName);

            if (!user.Success)
            {
                return new ErrorDataResult<List<Message>>(user.Message);
            }

            var values = new List<Message>();
            if (search == null)
            {
                values = await _messageDal.GetInboxWithMessageListAsync(user.Data.Id, filter, take, skip);
            }
            else
            {
                values = await _messageDal.GetInboxWithMessageListAsync(user.Data.Id, x => x.Subject.ToLower().Contains(search.ToLower()) || x.Details.ToLower().Contains(search.ToLower()), take, skip);
            }
            foreach (var item in values)
            {
                item.SenderUser.SenderUserInfo = null;
                item.Details = await TextFileManager.ReadTextFileAsync(item.Details, 50);
            }

            return new SuccessDataResult<List<Message>>(values.OrderByDescending(x => x.MessageDate).ToList());
        }

        [CacheRemoveAspect("IMessageService.Get")]
        [ValidationAspect(typeof(MessageValidator))]
        public async Task<IResult> AddMessageAsync(Message message, string senderUserName, string receiverUserName)
        {
            if (senderUserName == string.Empty || senderUserName == null)
            {
                return new ErrorResult(Messages.MessageSenderNotEmpty);
            }
            if (receiverUserName == string.Empty || receiverUserName == null)
            {
                return new ErrorResult(Messages.MessageReceiverNotEmpty);
            }

            var senderUser = await _userService.GetByUserNameAsync(senderUserName);
            if (!senderUser.Success)
            {
                return new ErrorResult(Messages.MessageSenderNotFound);
            }

            var receiverUser = await _userService.GetByUserNameAsync(receiverUserName);
            if (!receiverUser.Success)
            {
                return new ErrorResult(Messages.MessageReceiverNotFound);
            }

            IResult result = BusinessRules.Run(ReceiverUserNotEqualsSenderUser(senderUserName, receiverUserName), ReceiverUserNotEmpty(receiverUser), SenderUserNotEmpty(senderUser));

            if (!result.Success)
            {
                return result;
            }

            message.SenderUserId = senderUser.Data.Id;

            message.ReceiverUserId = receiverUser.Data.Id;
            message.Details = await TextFileManager.TextFileAddAsync(message.Details, ContentFileLocations.GetMessageContentFileLocation());
            message.MessageDate = DateTime.Now;
            await _messageDal.InsertAsync(message);
            return new SuccessResult();
        }

        [CacheRemoveAspect("IMessageService.Get")]
        public async Task<IResult> DeleteMessageAsync(int id, string userName)
        {
            var message = await GetByIdAsync(id);

            IResult result = BusinessRules.Run(MessageNotEmpty(message));

            if (!result.Success)
            {
                return result;
            }

            DeleteFileManager.DeleteFile(message.Data.Details);
            await _messageDal.DeleteAsync(message.Data);
            return new SuccessResult();
        }

        [CacheAspect]
        public async Task<IDataResult<Message>> GetByFilterAsync(Expression<Func<Message, bool>> filter = null)
        {
            var value = await _messageDal.GetByFilterAsync(filter);
            value.Details = await TextFileManager.ReadTextFileAsync(value.Details);
            return new SuccessDataResult<Message>(value);
        }

        [CacheAspect]
        public async Task<IDataResult<Message>> GetByIdAsync(int id)
        {
            var value = await _messageDal.GetByIDAsync(id);
            value.Details = await TextFileManager.ReadTextFileAsync(value.Details);
            return new SuccessDataResult<Message>(value);
        }

        [CacheRemoveAspect("IMessageService.Get")]
        public async Task<IResult> UpdateMessageAsync(Message t, string userName)
        {
            var user = await _userService.GetByUserNameAsync(userName);

            var result = BusinessRules.Run(ReceiverUserNotEmpty(user));

            if (!result.Success)
            {
                return result;
            }

            t.SenderUserId = user.Data.Id;
            DeleteFileManager.DeleteFile(t.Details);
            t.Details = await TextFileManager.TextFileAddAsync(t.Details, ContentFileLocations.GetMessageContentFileLocation());

            if (t.Details == null)
            {
                return new ErrorResult(Messages.MessageNotUpdating);
            }

            await _messageDal.UpdateAsync(t);
            return new SuccessResult();
        }

        [CacheAspect]
        public async Task<IDataResult<int>> GetCountAsync(Expression<Func<Message, bool>> filter = null)
        {
            return new SuccessDataResult<int>(await _messageDal.GetCountAsync(filter));
        }

        [CacheAspect]
        public async Task<IDataResult<int>> GetSendMessageCountAsync(string userName)
        {
            var sender = await _userService.GetByUserNameAsync(userName);
            return new SuccessDataResult<int>(await _messageDal.GetCountAsync(x => x.SenderUserId == sender.Data.Id));
        }

        [CacheAspect]
        public async Task<IDataResult<int>> GetUnreadMessagesCountByUserNameAsync(string userName)
        {
            var receiverUser = await _userService.GetByUserNameAsync(userName);

            IResult result = BusinessRules.Run(ReceiverUserNotEmpty(receiverUser));

            if (!result.Success)
            {
                return new ErrorDataResult<int>(result.Message);
            }

            var value = await GetCountAsync(x => x.ReceiverUserId == receiverUser.Data.Id && !x.MessageStatus);

            if (!value.Success)
            {
                return new ErrorDataResult<int>(value.Message);
            }

            return new SuccessDataResult<int>(value.Data);
        }

        [CacheAspect]
        public async Task<IDataResult<List<Message>>> GetSendBoxWithMessageListAsync(string userName, Expression<Func<Message, bool>> filter = null, int take = 0, int skip = 0)
        {
            var user = await _userService.GetByUserNameAsync(userName);
            var values = await _messageDal.GetSendBoxWithMessageListAsync(user.Data.Id, filter, take, skip);

            foreach (var item in values)
                item.Details = await TextFileManager.ReadTextFileAsync(item.Details, 50);
            return new SuccessDataResult<List<Message>>(values);
        }

        [CacheRemoveAspect("IMessageService.Get")]
        public async Task<IResult> MarkChangedAsync(int messageId, string userName)
        {
            IResult result = BusinessRules.Run(MessageIdNotEqualZero(messageId));

            if (!result.Success)
            {
                return result;
            }

            var message = await GetByFilterFileName(x => x.MessageID == messageId);
            var activeUser = await _userService.GetByUserNameAsync(userName);

            if (activeUser.Data.UserName != userName)
                return new ErrorResult();


            if (message.Data.MessageStatus)
                message.Data.MessageStatus = false;

            else
                message.Data.MessageStatus = true;

            await _messageDal.UpdateAsync(message.Data);
            return new SuccessResult();
        }

        public async Task<IDataResult<Message>> GetReceivedMessageAsync(string userName, Expression<Func<Message, bool>> filter = null)
        {
            var user = await _userService.GetByUserNameAsync(userName);

            IResult result = BusinessRules.Run(UserNotEmpty(user), ReceiverUserNotEmpty(user));

            if (!result.Success)
            {
                return new ErrorDataResult<Message>(result.Message);
            }

            var value = await _messageDal.GetReceivedMessageAsync(user.Data.Id, filter);
            if (value != null)
            {
                value.Details = await TextFileManager.ReadTextFileAsync(value.Details);
                if (!value.MessageStatus)
                {
                    await MarkUsReadAsync(value.MessageID, user.Data.UserName);
                }

                return new SuccessDataResult<Message>(value);
            }

            return new ErrorDataResult<Message>(Messages.MessageNotFound);
        }

        public async Task<IDataResult<Message>> GetSendMessageAsync(string userName, Expression<Func<Message, bool>> filter = null)
        {
            var user = await _userService.GetByUserNameAsync(userName);

            IResult result = BusinessRules.Run(UserNotEmpty(user));

            if (!result.Success)
            {
                return new ErrorDataResult<Message>(result.Message);
            }

            var value = await _messageDal.GetSendedMessageAsync(user.Data.Id, filter);
            if (value != null)
                value.Details = await TextFileManager.ReadTextFileAsync(value.Details);
            return new SuccessDataResult<Message>(value);
        }

        public async Task<IDataResult<Message>> GetByFilterFileName(Expression<Func<Message, bool>> filter = null)
        {
            return new SuccessDataResult<Message>(await _messageDal.GetByFilterAsync(filter));
        }

        [CacheRemoveAspect("IMessageService.Get")]
        public async Task<IResult> MarkUsReadAsync(int messageId, string userName)
        {
            var message = await GetByFilterFileName(x => x.MessageID == messageId);
            var activeUser = await _userService.GetByUserNameAsync(userName);

            IResult result = BusinessRules.Run(MessageIdNotEqualZero(messageId), UserNotEmpty(activeUser), ReceiverUserEqualActiveUser(userName, activeUser.Data.UserName));

            if (!result.Success)
            {
                return new ErrorResult(result.Message);
            }

            if (!message.Data.MessageStatus)
            {
                message.Data.MessageStatus = true;

                await _messageDal.UpdateAsync(message.Data);

                return new SuccessResult();
            }

            else
            {
                return new ErrorResult(Messages.MessageAlreadyRead);
            }

        }

        [CacheRemoveAspect("IMessageService.Get")]
        public async Task<IResult> MarkUsUnreadAsync(int messageId, string userName)
        {
            var message = await GetByFilterFileName(x => x.MessageID == messageId);
            var activeUser = await _userService.GetByUserNameAsync(userName);

            IResult result = BusinessRules.Run(MessageIdNotEqualZero(messageId), UserNotEmpty(activeUser), ReceiverUserEqualActiveUser(userName, activeUser.Data.UserName));

            if (!result.Success)
            {
                return new ErrorResult(result.Message);
            }

            if (message.Data.MessageStatus)
            {
                message.Data.MessageStatus = false;

                await _messageDal.UpdateAsync(message.Data);

                return new SuccessResult();
            }

            else
            {
                return new ErrorResult(Messages.MessageAlreadyUnread);
            }
        }

        [CacheRemoveAspect("IMessageService.Get")]
        public async Task<IResult> DeleteMessagesAsync(List<string> ids, string userName)
        {
            var user = await _userService.GetByUserNameAsync(userName);

            var result = BusinessRules.Run(MessageIdsNotEmpty(ids), UserNotEmpty(user));

            if (!result.Success)
            {
                return result;
            }

            List<Message> messages = new();

            foreach (var id in ids)
            {
                var message = await GetByIdAsync(Convert.ToInt32(id));
                if (message.Data.ReceiverUserId == user.Data.Id)
                {
                    DeleteFileManager.DeleteFile(message.Data.Details);
                    messages.Add(message.Data);
                }
            }

            await _messageDal.DeleteRangeAsync(messages);

            return new SuccessResult();
        }

        [CacheRemoveAspect("IMessageService.Get")]
        public async Task<IResult> MarksUsReadAsync(List<string> messageIds, string userName)
        {
            List<Message> messages = new();

            var activeUser = await _userService.GetByUserNameAsync(userName);

            var result = BusinessRules.Run(MessageIdsNotEmpty(messageIds), UserNotEmpty(activeUser));

            if (!result.Success)
            {
                return result;
            }

            foreach (var id in messageIds)
            {
                try
                {
                    var message = await GetByFilterFileName(x => x.MessageID == Convert.ToInt32(id));

                    if (activeUser.Data.Id == message.Data.ReceiverUserId)
                    {
                        if (!message.Data.MessageStatus)
                        {
                            message.Data.MessageStatus = true;

                            messages.Add(message.Data);
                        }
                    }
                }
                catch
                {
                    continue;
                }
            }
            await _messageDal.UpdateRangeAsync(messages);
            return new SuccessResult();
        }

        [CacheRemoveAspect("IMessageService.Get")]
        public async Task<IResult> MarksUsUnreadAsync(List<string> messageIds, string userName)
        {
            List<Message> messages = new();

            var activeUser = await _userService.GetByUserNameAsync(userName);

            var result = BusinessRules.Run(MessageIdsNotEmpty(messageIds), UserNotEmpty(activeUser));

            if (!result.Success)
            {
                return result;
            }

            foreach (var id in messageIds)
            {
                try
                {
                    var message = await GetByFilterFileName(x => x.MessageID == Convert.ToInt32(id));

                    if (activeUser.Data.UserName == userName)
                    {
                        if (message.Data.MessageStatus)
                        {
                            message.Data.MessageStatus = false;

                            messages.Add(message.Data);
                        }
                    }
                }
                catch
                {
                    continue;
                }
            }
            await _messageDal.UpdateRangeAsync(messages);
            return new SuccessResult();
        }


        //Business Rules

        private IResult ReceiverUserNotEqualsSenderUser(string receiverUser, string senderUser)
        {
            if (receiverUser == senderUser)
            {
                return new ErrorResult(Messages.MessageReceiverNotEqualsSender);
            }
            return new SuccessResult();
        }

        private IResult ReceiverUserNotEmpty(IDataResult<UserDto> receiverUser)
        {
            if (!receiverUser.Success)
            {
                return new ErrorResult(Messages.MessageReceiverNotEmpty);
            }
            return new SuccessResult();
        }

        private IResult SenderUserNotEmpty(IDataResult<UserDto> senderUser)
        {
            if (!senderUser.Success)
            {
                return new ErrorResult(Messages.MessageSenderNotEmpty);
            }
            return new SuccessResult();
        }

        private IResult MessageNotEmpty(IDataResult<Message> message)
        {
            if (message.Data == null)
            {
                return new ErrorResult(Messages.MessageNotEmpty);
            }
            return new SuccessResult();
        }

        private IResult MessageIdNotEqualZero(int id)
        {
            if (id == 0)
            {
                return new ErrorResult(Messages.MessageNotEmpty);
            }
            return new SuccessResult();
        }

        private IResult MessageIdsNotEmpty(List<string> Ids)
        {
            if (Ids == null)
            {
                return new ErrorResult(Messages.MessagesNotEmpty);
            }
            return new SuccessResult();
        }

        private IResult ReceiverUserEqualActiveUser(string receiverUserName, string activeUserName)
        {
            if (receiverUserName != activeUserName)
            {
                return new ErrorResult(Messages.MessageDoesNotBelongToTheUser);
            }
            return new SuccessResult();
        }
    }
}
