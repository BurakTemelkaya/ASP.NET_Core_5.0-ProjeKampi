using CoreLayer.DataAccess;
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
    public class EfBlogRepository : EfEntityRepositoryBase<Blog>, IBlogDal
    {
        public EfBlogRepository(Context context) : base(context)
        {

        }

        private Context Context
        {
            get
            {
                return _context as Context;
            }
        }

        public async Task<List<BlogCategoryandCommentCountDto>> GetBlogListWithCategoryandCommentCountAsync(Expression<Func<BlogCategoryandCommentCountDto, bool>> filter = null, int take = 0, int skip = 0)
        {
            var query =
            Context.Blogs
            .Include(x => x.Category)
            .Include(x => x.Comments)
            .Select(blog =>
            new BlogCategoryandCommentCountDto
            {
                BlogID = blog.BlogID,
                BlogContent = blog.BlogContent,
                BlogImage = blog.BlogImage,
                BlogThumbnailImage = blog.BlogThumbnailImage,
                BlogTitle = blog.BlogTitle,
                BlogCreateDate = blog.BlogCreateDate,
                BlogStatus = blog.BlogStatus,
                CategoryID = blog.Category.CategoryID,
                CategoryName = blog.Category.CategoryName,
                CategoryStatus = blog.Category.CategoryStatus,
                CommentCount = blog.Comments.Count > 0 ? blog.Comments.Count : 0,
                CommentScore = blog.Comments.Count > 0 ? blog.Comments.Average(x => x.BlogScore) : 0
            })
            .OrderByDescending(x => x.BlogID).AsQueryable();

            query = filter != null ? query.Where(filter) : query;

            query = take != 0 ? query.Skip(skip).Take(take) : query;

            return await query.ToListAsync();
        }

        public async Task<List<BlogCategoryandCommentCountDto>> GetListWithCategoryandCommentCountByPagingAsync(Expression<Func<BlogCategoryandCommentCountDto, bool>> filter = null, int take = 0, int page = 1)
        {
            int skip = 0;
            if (page > 1)
            {
                skip = take * (page - 1);
            }

            int count = await GetCountByBlogCategoryandCommentCountAsync(filter);

            if (skip >= count)
            {
                skip = 0;
                page = 1;
            }

            return AddNullObject<BlogCategoryandCommentCountDto>.GetListByPaging(await GetBlogListWithCategoryandCommentCountAsync(filter, take, skip), take, page, count);
        }

        public async Task<List<Blog>> GetListWithCategoryByWriterAsync(int id, Expression<Func<Blog, bool>> filter = null, int take = 0, int skip = 0)
        {
            var query = Context.Blogs.Include(x => x.Category)
                .Where(x => x.WriterID == id)
                    .OrderByDescending(x => x.BlogID)
                    .Skip(skip)
                    .Take(take)
                        .Where(x => x.WriterID == id)
                            .AsQueryable();

            return filter != null ?
                await query.Where(filter)
                .ToListAsync()
                : await query.ToListAsync();
        }

        public async Task<List<Blog>> GetListWithCategoryByWriterandPagingAsync(int id, Expression<Func<Blog, bool>> filter = null, int take = 0, int page = 1)
        {
            int skip = 0;
            if (page > 1)
            {
                skip = take * (page - 1);
            }

            int count = await GetCountAsync(x => x.WriterID == id);

            if (skip >= count)
            {
                skip = 0;
                page = 1;
            }

            return AddNullObject<Blog>.GetListByPaging(await GetListWithCategoryByWriterAsync(id, filter, take, skip), take, page, count);
        }

        public async Task<Blog> GetBlogByIdWithCommentandWriterAsync(int id, bool isCommentStatus, Expression<Func<Blog, bool>> filter = null)
        {
            try
            {
                return await Context.Blogs
                .Include(x => x.Comments.Where(x => x.CommentStatus == isCommentStatus))
                .Include(u => u.Writer)
                    .Where(x => x.BlogID == id)
                    .Where(filter).FirstAsync();
            }
            catch
            {
                return null;
            }
        }

        public async Task<int> GetCountByBlogCategoryandCommentCountAsync(Expression<Func<BlogCategoryandCommentCountDto, bool>> filter = null)
        {
            var query = Context.Blogs.Select(data =>
                new BlogCategoryandCommentCountDto
                {
                    BlogID = data.BlogID,
                    BlogContent = data.BlogContent,
                    BlogImage = data.BlogImage,
                    BlogTitle = data.BlogTitle,
                    BlogCreateDate = data.BlogCreateDate,
                    BlogThumbnailImage = data.BlogThumbnailImage,
                    BlogStatus = data.BlogStatus,
                    CategoryID = data.CategoryID,
                    CategoryName = data.Category.CategoryName,
                    CategoryStatus = data.Category.CategoryStatus
                });

            query = filter != null ? query.Where(filter) : query;

            return await query.CountAsync();
        }
    }
}
