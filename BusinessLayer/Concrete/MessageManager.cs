using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
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
            return await _messageDal.GetInboxWithMessageListAsync(id, filter);
        }

        public async Task<List<Message>> GetListAsync(Expression<Func<Message, bool>> filter = null)
        {
            return await _messageDal.GetListAllAsync(filter);
        }

        public async Task TAddAsync(Message t)
        {
            t.MessageDate = DateTime.Now;
            t.MessageStatus = true;
            await _messageDal.InsertAsync(t);
        }

        public async Task TDeleteAsync(Message t)
        {
            await _messageDal.DeleteAsync(t);
        }

        public async Task<Message> TGetByFilterAsync(Expression<Func<Message, bool>> filter = null)
        {
            return await _messageDal.GetByFilterAsync(filter);
        }

        public async Task<Message> TGetByIDAsync(int id)
        {
            return await _messageDal.GetByIDAsync(id);
        }

        public async Task TUpdateAsync(Message t)
        {
            await _messageDal.UpdateAsync(t);
        }

        public async Task<int> GetCountAsync(Expression<Func<Message, bool>> filter = null)
        {
            return await _messageDal.GetCountAsync(filter);
        }

        public async Task<List<Message>> GetSendBoxWithMessageListAsync(int id, Expression<Func<Message, bool>> filter = null)
        {
            return await _messageDal.GetSendBoxWithMessageListAsync(id, filter);
        }

        public async Task<bool> MarkChangedAsync(int messageId, string userName)
        {
            if (messageId != 0)
            {
                var message = await TGetByFilterAsync(x => x.MessageID == messageId);
                var activeUser = await _userService.FindByUserNameAsync(userName);
                if (activeUser.UserName != userName)
                {
                    return false;
                }
                if (message.MessageStatus)
                    message.MessageStatus = false;
                else
                    message.MessageStatus = true;
                await TUpdateAsync(message);
                return true;
            }
            return false;
        }

        public async Task<Message> GetReceivedMessageAsync(int id, Expression<Func<Message, bool>> filter = null)
        {
            return await _messageDal.GetReceivedMessageAsync(id, filter);
        }

        public async Task<Message> GetSendMessageAsync(int id, Expression<Func<Message, bool>> filter = null)
        {
            return await _messageDal.GetSendedMessageAsync(id, filter);
        }
    }
}
