﻿using CoreLayer.DataAccess.EntityFramework;
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
            Context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        private Context Context
        {
            get
            {
                return _context as Context;
            }
        }

        public async Task<List<CategoryBlogandBlogCountDto>> GetListWithCategoryByBlog(Expression<Func<CategoryBlogandBlogCountDto, bool>> filter = null)
        {
            var query = Context.Categories.Include(x => x.Blogs)
                .SelectMany(category => category.Blogs, (category, blog) =>
                new CategoryBlogandBlogCountDto
                {
                    CategoryID = category.CategoryID,
                    CategoryName = category.CategoryName,
                    CategoryStatus = category.CategoryStatus,
                    CategoryDescription = category.CategoryDescription,
                    BlogStatus = blog.BlogStatus
                });

            query = filter != null ? query.Where(filter) : query;

            query = query.GroupBy(data => new
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
                CategoryDescription = data.Key.CategoryDescription,
                NumberofBloginCategory = data.Count(),
                BlogStatus = data.Key.BlogStatus
            });

            return await query.ToListAsync();
        }
    }
}
