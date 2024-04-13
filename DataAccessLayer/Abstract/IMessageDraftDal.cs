using CoreLayer.DataAccess;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IMessageDraftDal : IEntityRepository<MessageDraft>
    {
        Task<List<MessageDraft>> GetMessageDraftListAsync(Expression<Func<MessageDraft, bool>> filter = null);
        Task<List<MessageDraft>> GetMessageDraftListByUserIdAsync(int id, Expression<Func<MessageDraft, bool>> filter = null);
        Task<MessageDraft> GetMessageDraftByUserIdAsync(int id, Expression<Func<MessageDraft, bool>> filter = null);
    }
}
