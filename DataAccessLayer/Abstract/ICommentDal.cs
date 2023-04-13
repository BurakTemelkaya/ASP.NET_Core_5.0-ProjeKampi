using CoreLayer.DataAccess;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface ICommentDal : IEntityRepository<Comment>
    {
        Task<List<Comment>> GetListWithCommentByBlogAsync(Expression<Func<Comment, bool>> filter = null, int take = 0, int skip = 0);

        Task<List<Comment>> GetListWithCommentByBlogandPagingAsync(Expression<Func<Comment, bool>> filter = null, int take = 0, int page = 1);

        Task<Comment> GetCommentByBlog(Expression<Func<Comment, bool>> filter = null);
    }
}
