using CoreLayer.Utilities.Results;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface ICommentService
    {
        Task<IResultObject> TAddAsync(Comment t);

        Task<IResultObject> TDeleteAsync(Comment t);

        Task<IResultObject> TUpdateAsync(Comment t);

        Task<IDataResult<Comment>> TGetByIDAsync(int id);

        Task<IDataResult<int>> GetCountAsync(int blogScore = 0);

        Task<IDataResult<List<Comment>>> GetListByBlogIdAsync(int id);

        Task<IDataResult<List<Comment>>> GetBlogListWithCommentAsync();

        Task<IDataResult<List<Comment>>> GetCommentListWithBlogByPagingAsync(int take = 0, int page = 1);

        Task<IDataResult<List<Comment>>> GetCommentListByWriterandPaging(string userName, int take, int page);

        Task<IDataResult<Comment>> GetByIdandWriterAsync(string userName, int id);

        Task<IResultObject> DeleteCommentByWriter(string userName, int id);

        Task<IResultObject> DisabledCommentByWriter(string userName, int id);

        Task<IResultObject> EnabledCommentByWriter(string userName, int id);

        Task<IResultObject> ChangeStatusCommentByWriter(string userName, int id);

        Task<IResultObject> ChangeStatusCommentByAdmin(int id);
    }
}
