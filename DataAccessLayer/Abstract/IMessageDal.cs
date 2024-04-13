using CoreLayer.DataAccess;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IMessageDal : IEntityRepository<Message>
    {
        Task<List<MessageSenderUserDto>> GetInboxWithMessageListAsync(int id, Expression<Func<MessageSenderUserDto, bool>> filter = null, int take = 0, int skip = 0);
        Task<List<MessageReceiverUserDto>> GetSendBoxWithMessageListAsync(int id, Expression<Func<MessageReceiverUserDto, bool>> filter = null, int take = 0, int skip = 0);
        Task<Message> GetReceivedMessageAsync(int id, Expression<Func<Message, bool>> filter = null);
        Task<Message> GetSendedMessageAsync(int id, Expression<Func<Message, bool>> filter = null);
    }
}
