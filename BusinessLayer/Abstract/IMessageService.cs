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
        Task<List<Message>> GetInboxWithMessageListAsync(int id, Expression<Func<Message, bool>> filter = null);
        Task<List<Message>> GetSendBoxWithMessageListAsync(int id, Expression<Func<Message, bool>> filter = null);
        Task<Message> GetReceivedMessageAsync(int id, Expression<Func<Message, bool>> filter = null);
        Task<Message> GetSendMessageAsync(int id, Expression<Func<Message, bool>> filter = null);
        Task<bool> MarkChangedAsync(int messageId, string userName);
    }
}
