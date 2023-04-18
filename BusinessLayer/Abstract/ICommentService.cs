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
    public interface ICommentService
    {
        Task<IResult> TAddAsync(Comment t);

        Task<IResult> TDeleteAsync(Comment t);

        Task<IResult> TUpdateAsync(Comment t);

        Task<IDataResult<List<Comment>>> GetListAsync(Expression<Func<Comment, bool>> filter = null);

        Task<IDataResult<Comment>> TGetByIDAsync(int id);

        Task<IDataResult<Comment>> TGetByFilterAsync(Expression<Func<Comment, bool>> filter = null);

        Task<IDataResult<int>> GetCountAsync(Expression<Func<Comment, bool>> filter = null);

        Task<IDataResult<List<Comment>>> GetListByBlogIdAsync(int id);

        Task<IDataResult<List<Comment>>> GetBlogListWithCommentAsync();

        Task<IDataResult<List<Comment>>> GetCommentListWithBlogByPagingAsync(int take = 0, int page = 0);

        Task<IDataResult<List<Comment>>> GetCommentListByWriterandPaging(string userName, int take, int page);

        Task<IDataResult<Comment>> GetByIdandWriterAsync(string userName, int id);

        Task<IResult> DeleteCommentByWriter(string userName, int id);

        Task<IResult> DisabledCommentByWriter(string userName, int id);

        Task<IResult> EnabledCommentByWriter(string userName, int id);

        Task<IResult> ChangeStatusCommentByWriter(string userName, int id);

        Task<IResult> ChangeStatusCommentByAdmin(int id);
    }
}
