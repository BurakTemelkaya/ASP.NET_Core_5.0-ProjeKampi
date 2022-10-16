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

        public List<Message> GetInboxWithMessageList(int id, Expression<Func<Message, bool>> filter = null)
        {
            return _messageDal.GetInboxWithMessageList(id, filter);
        }

        public List<Message> GetList(Expression<Func<Message, bool>> filter = null)
        {
            return _messageDal.GetListAll(filter);
        }

        public void TAdd(Message t)
        {
            t.MessageDate = DateTime.Now;
            t.MessageStatus = true;
            _messageDal.Insert(t);
        }

        public void TDelete(Message t)
        {
            _messageDal.Delete(t);
        }

        public Message TGetByFilter(Expression<Func<Message, bool>> filter = null)
        {
            return _messageDal.GetByFilter(filter);
        }

        public Message TGetByID(int id)
        {
            return _messageDal.GetByID(id);
        }

        public void TUpdate(Message t)
        {
            _messageDal.Update(t);
        }

        public int GetCount(Expression<Func<Message, bool>> filter = null)
        {
            return _messageDal.GetCount(filter);
        }

        public List<Message> GetSendBoxWithMessageList(int id, Expression<Func<Message, bool>> filter = null)
        {
            return _messageDal.GetSendBoxWithMessageList(id, filter);
        }

        public async Task<bool> MarkChangedAsync(int messageId, string userName)
        {
            if (messageId != 0)
            {
                var message = TGetByFilter(x => x.MessageID == messageId);
                var activeUser = await _userService.FindByUserNameAsync(userName);
                if (activeUser.UserName != userName)
                {
                    return false;
                }
                if (message.MessageStatus)
                    message.MessageStatus = false;
                else
                    message.MessageStatus = true;
                TUpdate(message);
                return true;
            }
            return false;
        }

        public Message GetReceivedMessage(int id, Expression<Func<Message, bool>> filter = null)
        {
            return _messageDal.GetReceivedMessage(id, filter);
        }

        public Message GetSendMessage(int id, Expression<Func<Message, bool>> filter = null)
        {
            return _messageDal.GetSendedMessage(id, filter);
        }
    }
}
