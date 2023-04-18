using CoreLayer.DataAccess;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IMessageDal : IEntityRepository<Message>
    {
        Task<List<Message>> GetInboxWithMessageListAsync(int id, Expression<Func<Message, bool>> filter = null, int take = 0, int skip = 0);
        Task<List<Message>> GetSendBoxWithMessageListAsync(int id, Expression<Func<Message, bool>> filter = null, int take = 0, int skip = 0);
        Task<Message> GetReceivedMessageAsync(int id, Expression<Func<Message, bool>> filter = null);
        Task<Message> GetSendedMessageAsync(int id, Expression<Func<Message, bool>> filter = null);
    }
}
