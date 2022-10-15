using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IMessageService : IGenericService<Message>
    {
        List<Message> GetInboxWithMessageList(int id, Expression<Func<Message, bool>> filter = null);
        List<Message> GetSendBoxWithMessageList(int id, Expression<Func<Message, bool>> filter = null);
        Message GetReceivedMessage(int id, Expression<Func<Message, bool>> filter = null);
        Message GetSendMessage(int id, Expression<Func<Message, bool>> filter = null);
        Task<bool> MarkChangedAsync(int messageId, string userName);
    }
}
