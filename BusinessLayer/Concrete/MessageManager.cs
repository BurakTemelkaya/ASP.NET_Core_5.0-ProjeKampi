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
    public class MessageManager : IMessageService
    {
        private readonly IMessageDal _messageDal;
        private readonly IBusinessUserService _userService;

        public MessageManager(IMessageDal message2Dal, IBusinessUserService userService)
        {
            _messageDal = message2Dal;
            _userService = userService;
        }

        public async Task<List<Message>> GetInboxWithMessageListAsync(int id, Expression<Func<Message, bool>> filter = null)
        {
            var values = await _messageDal.GetInboxWithMessageListAsync(id, filter);
            foreach (var item in values)
                item.Details = await TextFileManager.ReadTextFileAsync(item.Details, 30);
            return values;
        }

        public async Task<List<Message>> GetListAsync(Expression<Func<Message, bool>> filter = null)
        {
            var values = await _messageDal.GetListAllAsync(filter);
            foreach (var item in values)
                item.Details = await TextFileManager.ReadTextFileAsync(item.Details, 30);
            return values;
        }

        public async Task AddMessageAsync(Message message, string senderUserName, string receiverUserName)
        {
            if (senderUserName == receiverUserName)
                return;
            var senderUser = await _userService.FindByUserNameAsync(senderUserName);
            if (senderUser == null)
                return;
            message.SenderUserId = senderUser.Id;
            var receiverUser = await _userService.FindByUserNameAsync(receiverUserName);
            if (receiverUser == null)
                return;
            message.ReceiverUserId = receiverUser.Id;
            message.Details = await TextFileManager.TextFileAddAsync(message.Details, TextFileManager.GetMessageContentFileLocation());
            message.MessageDate = DateTime.Now;
            message.MessageStatus = true;
            await _messageDal.InsertAsync(message);
        }

        public async Task DeleteMessageAsync(int id, string userName)
        {
            var message = await GetByIdAsync(id);
            DeleteFileManager.DeleteFile(message.Details);
            await _messageDal.DeleteAsync(message);
        }

        public async Task<Message> GetByFilterAsync(Expression<Func<Message, bool>> filter = null)
        {
            var value = await _messageDal.GetByFilterAsync(filter);
            value.Details = await TextFileManager.ReadTextFileAsync(value.Details);
            return value;
        }

        public async Task<Message> GetByIdAsync(int id)
        {
            var value = await _messageDal.GetByIDAsync(id);
            value.Details = await TextFileManager.ReadTextFileAsync(value.Details);
            return value;
        }

        public async Task UpdateMessageAsync(Message t, string userName)
        {
            var user = await _userService.FindByUserNameAsync(userName);
            if (user != null)
                return;
            t.SenderUserId = user.Id;
            DeleteFileManager.DeleteFile(t.Details);
            t.Details = await TextFileManager.TextFileAddAsync(t.Details, TextFileManager.GetMessageContentFileLocation());
            await _messageDal.UpdateAsync(t);
        }

        public async Task<int> GetCountAsync(Expression<Func<Message, bool>> filter = null)
        {
            return await _messageDal.GetCountAsync(filter);
        }

        public async Task<int> GetUnreadMessagesCountByUserNameAsync(string userName)
        {
            var receiverUser = await _userService.FindByUserNameAsync(userName);
            return await GetCountAsync(x => x.ReceiverUserId == receiverUser.Id && x.MessageStatus == false);
        }

        public async Task<List<Message>> GetSendBoxWithMessageListAsync(int id, Expression<Func<Message, bool>> filter = null)
        {
            var values = await _messageDal.GetSendBoxWithMessageListAsync(id, filter);
            foreach (var item in values)
                item.Details = await TextFileManager.ReadTextFileAsync(item.Details, 30);
            return values;
        }

        public async Task<bool> MarkChangedAsync(int messageId, string userName)
        {
            if (messageId != 0)
            {
                var message = await GetByFilterFileName(x => x.MessageID == messageId);
                var activeUser = await _userService.FindByUserNameAsync(userName);

                if (activeUser.UserName != userName)
                    return false;


                if (message.MessageStatus)
                    message.MessageStatus = false;

                else
                    message.MessageStatus = true;

                await _messageDal.UpdateAsync(message);
                return true;
            }
            return false;
        }

        public async Task<Message> GetReceivedMessageAsync(int id, Expression<Func<Message, bool>> filter = null)
        {
            var value = await _messageDal.GetReceivedMessageAsync(id, filter);
            if (value != null)
                value.Details = await TextFileManager.ReadTextFileAsync(value.Details);
            return value;
        }

        public async Task<Message> GetSendMessageAsync(int id, Expression<Func<Message, bool>> filter = null)
        {
            var value = await _messageDal.GetSendedMessageAsync(id, filter);
            if (value != null)
                value.Details = await TextFileManager.ReadTextFileAsync(value.Details);
            return value;
        }

        public async Task<Message> GetByFilterFileName(Expression<Func<Message, bool>> filter = null)
        {
            return await _messageDal.GetByFilterAsync(filter);
        }

        public async Task<bool> MarkUsReadAsync(int messageId, string userName)
        {
            if (messageId != 0)
            {
                var message = await GetByFilterFileName(x => x.MessageID == messageId);
                var activeUser = await _userService.FindByUserNameAsync(userName);

                if (activeUser.UserName != userName)
                {
                    return false;
                }

                if (!message.MessageStatus)
                    message.MessageStatus = true;

                await _messageDal.UpdateAsync(message);
                return true;
            }
            return false;
        }

        public async Task<bool> MarkUsUnreadAsync(int messageId, string userName)
        {
            if (messageId != 0)
            {
                var message = await GetByFilterFileName(x => x.MessageID == messageId);
                var activeUser = await _userService.FindByUserNameAsync(userName);

                if (activeUser.UserName != userName)
                {
                    return false;
                }

                if (message.MessageStatus)
                    message.MessageStatus = false;

                await _messageDal.UpdateAsync(message);
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteMessagesAsync(List<string> ids, string userName)
        {
            if (ids == null || userName == null)
            {
                return false;
            }

            List<Message> messages = new();

            foreach (var id in ids)
            {
                var message = await GetByIdAsync(Convert.ToInt32(id));
                DeleteFileManager.DeleteFile(message.Details);
                messages.Add(message);
            }
            await _messageDal.DeleteRangeAsync(messages);
            return true;
        }

        public async Task<bool> MarksUsReadAsync(List<string> messageIds, string userName)
        {
            List<Message> messages = new();

            foreach (var id in messageIds)
            {
                if (Convert.ToInt32(id) != 0)
                {
                    var message = await GetByFilterFileName(x => x.MessageID == Convert.ToInt32(id));
                    var activeUser = await _userService.FindByUserNameAsync(userName);

                    if (activeUser.UserName != userName)
                    {
                        return false;
                    }

                    if (!message.MessageStatus)
                        message.MessageStatus = true;

                    messages.Add(message);
                }
                else
                {
                    return false;
                }
            }
            await _messageDal.UpdateRangeAsync(messages);
            return true;
        }

        public async Task<bool> MarksUsUnreadAsync(List<string> messageIds, string userName)
        {
            List<Message> messages = new();

            foreach (var id in messageIds)
            {
                if (Convert.ToInt32(id) != 0)
                {
                    var message = await GetByFilterFileName(x => x.MessageID == Convert.ToInt32(id));
                    var activeUser = await _userService.FindByUserNameAsync(userName);

                    if (activeUser.UserName != userName)
                    {
                        return false;
                    }

                    if (message.MessageStatus)
                        message.MessageStatus = false;

                    messages.Add(message);
                }
                else
                {
                    return false;
                }
            }
            await _messageDal.UpdateRangeAsync(messages);
            return true;
        }
    }
}
