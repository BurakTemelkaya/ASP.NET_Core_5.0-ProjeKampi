using CoreLayer.Utilities.Results;
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
        Task<IDataResult<List<MessageDraft>>> GetMessageDraftListAsync(Expression<Func<MessageDraft, bool>> filter = null);
        Task<IDataResult<List<MessageDraft>>> GetMessageDraftListByUserNameAsync(string userName, Expression<Func<MessageDraft, bool>> filter = null, int length = 30);
        Task<IDataResult<int>> GetCountAsync(Expression<Func<MessageDraft, bool>> filter = null);
        Task<IDataResult<int>> GetCountByUserNameAsync(string userName);
        Task<IDataResult<List<MessageDraft>>> GetListAsync(Expression<Func<MessageDraft, bool>> filter = null);
        Task<IResult> AddAsync(MessageDraft t, string userName);
        Task<IResult> DeleteAsync(int id, string userName);
        Task<IResult> DeleteMessageDraftsAsync(List<string> ids, string userName);
        Task<IDataResult<MessageDraft>> GetByFilterAsync(string userName, Expression<Func<MessageDraft, bool>> filter = null);
        Task<IDataResult<MessageDraft>> GetByIDAsync(int id, string userName);
        Task<IResult> UpdateAsync(MessageDraft t, string userName);
    }
}
