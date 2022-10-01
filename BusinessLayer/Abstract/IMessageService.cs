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
        List<Message> GetInboxWithMessageByWriter(int id);
        List<Message> GetSendBoxWithMessageByWriter(int id);
        Task<bool> MarkChangedAsync(int messageId, string userName);
    }
}
