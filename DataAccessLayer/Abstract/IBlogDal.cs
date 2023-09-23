using CoreLayer.DataAccess;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IBlogDal : IEntityRepository<Blog>
    {
        Task<List<BlogCategoryandCommentCountDto>> GetBlogListWithCategoryandCommentCountAsync(Expression<Func<BlogCategoryandCommentCountDto, bool>> filter = null, bool commentStatus = true, int take = 0, int skip = 0);

        Task<List<BlogCategoryandCommentCountDto>> GetListWithCategoryandCommentCountByPagingAsync(Expression<Func<BlogCategoryandCommentCountDto, bool>> filter = null, bool commentStatus = true, int take = 0, int page = 1);

        Task<List<Blog>> GetListBlogWithCategoryAsync(Expression<Func<Blog, bool>> filter = null, int take = 0, int skip = 0);

        Task<List<Blog>> GetListWithCategoryByPagingAsync(Expression<Func<Blog, bool>> filter = null, int take = 0, int page = 1);

        Task<Blog> GetBlogWithCommentandWriterAsync(bool isCommentStatus, Expression<Func<Blog, bool>> filter = null);

        Task<int> GetCountByBlogCategoryandCommentCountAsync(Expression<Func<BlogCategoryandCommentCountDto, bool>> filter = null);
    }
}
