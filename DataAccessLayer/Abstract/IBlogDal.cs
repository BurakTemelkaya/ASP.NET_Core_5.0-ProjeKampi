using CoreLayer.DataAccess;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using X.PagedList;

namespace DataAccessLayer.Abstract;

public interface IBlogDal : IEntityRepository<Blog>
{
    Task<IPagedList<BlogCategoryandCommentCountDto>> GetBlogListWithCategoryandCommentCountAsync(Expression<Func<BlogCategoryandCommentCountDto, bool>> filter = null, bool commentStatus = true, int pageNumber = 1, int pageSize = 10);

    Task<BlogCategoryandCommentCountandWriterDto> GetBlogWithCommentandWriterAsync(bool isCommentStatus, Expression<Func<BlogCategoryandCommentCountandWriterDto, bool>> filter = null);

    Task<int> GetCountByBlogCategoryandCommentCountAsync(Expression<Func<BlogCategoryandCommentCountDto, bool>> filter = null);
}
