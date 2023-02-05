using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IMessageDraftService
    {
        Task<List<MessageDraft>> GetMessageDraftListAsync(Expression<Func<MessageDraft, bool>> filter = null);
        Task<List<MessageDraft>> GetMessageDraftListByUserNameAsync(string userName, Expression<Func<MessageDraft, bool>> filter = null);
        Task<int> GetCountAsync(Expression<Func<MessageDraft, bool>> filter = null);
        Task<int> GetCountByUserNameAsync(string userName);
        Task<List<MessageDraft>> GetListAsync(Expression<Func<MessageDraft, bool>> filter = null);
        Task AddAsync(MessageDraft t, string userName);
        Task DeleteAsync(int id, string userName);
        Task<MessageDraft> GetByFilterAsync(string userName, Expression<Func<MessageDraft, bool>> filter = null);
        Task<MessageDraft> GetByIDAsync(int id, string userName);
        Task UpdateAsync(MessageDraft t, string userName);
    }
}
