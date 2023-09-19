using CoreLayer.DataAccess.EntityFramework;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.EntityFramework
{
    public class EfCategoryRepository : EfEntityRepositoryBase<Category>, ICategoryDal
    {
        public EfCategoryRepository(Context context) : base(context)
        {

        }

        private Context Context
        {
            get
            {
                return _context as Context;
            }
        }

        public async Task<List<CategoryBlogandBlogCountDto>> GetListWithCategoryByBlog(bool categoryStatus = true, bool blogStatus = true)
        {
            var data = await Context.Categories.Include(x => x.Blogs)
                .SelectMany(category => category.Blogs, (category, blog) =>
                new
                {
                    category.CategoryID,
                    category.CategoryName,
                    category.CategoryStatus,
                    category.CategoryDescription,
                    blog.BlogStatus
                })
                .Where(x => x.CategoryStatus == categoryStatus && x.BlogStatus == blogStatus)
                .GroupBy(data => new
                {
                    data.CategoryName,
                    data.CategoryStatus,
                    data.CategoryID,
                    data.CategoryDescription,
                    data.BlogStatus,
                })
                .Select(data => new CategoryBlogandBlogCountDto
                {
                    CategoryID = data.Key.CategoryID,
                    CategoryName = data.Key.CategoryName,
                    NumberofBloginCategory = data.Count(),
                    BlogStatus = data.Key.BlogStatus
                })
                .ToListAsync();

            return data;
        }
    }
}
