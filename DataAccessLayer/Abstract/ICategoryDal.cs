using CoreLayer.DataAccess;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EntityLayer.DTO;

namespace DataAccessLayer.Abstract
{
    public interface ICategoryDal : IEntityRepository<Category>
    {
        Task<List<CategoryBlogandBlogCountDto>> GetListWithCategoryByBlog(Expression<Func<CategoryBlogandBlogCountDto, bool>> filter = null);
    }
}
