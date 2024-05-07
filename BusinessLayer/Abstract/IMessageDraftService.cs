using CoreLayer.Utilities.Results;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IMessageDraftService
    {
        Task<IDataResult<List<MessageDraft>>> GetMessageDraftListByUserNameAsync(string userName, int length = 30);
        Task<IDataResult<int>> GetCountAsync(Expression<Func<MessageDraft, bool>> filter = null);
        Task<IDataResult<int>> GetCountByUserNameAsync(string userName);
        Task<IResultObject> AddAsync(MessageDraft t, string userName);
        Task<IResultObject> DeleteAsync(int id, string userName);
        Task<IResultObject> DeleteMessageDraftsAsync(List<string> ids, string userName);
        Task<IDataResult<MessageDraft>> GetByIDAsync(int id, string userName);
        Task<IResultObject> UpdateAsync(MessageDraft t, string userName);
    }
}
