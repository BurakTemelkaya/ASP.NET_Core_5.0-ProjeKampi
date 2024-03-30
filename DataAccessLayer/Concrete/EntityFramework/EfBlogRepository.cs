using CoreLayer.DataAccess;
using CoreLayer.DataAccess.EntityFramework;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.EntityFramework
{
    public class EfBlogRepository : EfEntityRepositoryBase<Blog>, IBlogDal
    {
        public EfBlogRepository(Context context) : base(context)
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

        public async Task<List<BlogCategoryandCommentCountDto>> GetBlogListWithCategoryandCommentCountAsync(Expression<Func<BlogCategoryandCommentCountDto, bool>> filter = null, bool commentStatus = true, int take = 0, int skip = 0)
        {
            var leftQuery = from blog in Context.Blogs
                            join category in Context.Categories
                                on blog.CategoryID equals category.CategoryID
                            join comment in Context.Comments.Where(x => x.CommentStatus == commentStatus)
                                on blog.BlogID equals comment.BlogID into BlogComments
                            from comment in BlogComments.DefaultIfEmpty()
                            select new BlogCategoryandCommentCountDto
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
                                CategoryStatus = category.CategoryStatus,
                                CommentCount = comment != null ? blog.Comments.Count(x => x.CommentStatus) : 0,
                                CommentScore = comment != null ? blog.Comments.Where(x => x.CommentStatus).Average(x => x.BlogScore) : 0,
                                CommentStatus = comment == null ? false : comment.CommentStatus
                            };

            var rightQuery = from comment in Context.Comments.Where(x => x.CommentStatus == commentStatus)
                             join category in Context.Categories
                                 on comment.Blog.CategoryID equals category.CategoryID
                             join blog in Context.Blogs
                                 on comment.BlogID equals blog.BlogID into BlogComments
                             from blog in BlogComments.DefaultIfEmpty()
                             select new BlogCategoryandCommentCountDto
                             {
                                 BlogID = blog.BlogID,
                                 BlogContent = blog.BlogContent,
                                 BlogImage = blog.BlogImage,
                                 BlogThumbnailImage = blog.BlogThumbnailImage,
                                 BlogTitle = blog.BlogTitle,
                                 BlogCreateDate = blog.BlogCreateDate,
                                 BlogStatus = blog.BlogStatus,
                                 CategoryID = category.CategoryID,
                                 CategoryName = category.CategoryName,
                                 CategoryStatus = category.CategoryStatus,
                                 CommentCount = comment != null ? blog.Comments.Count(x => x.CommentStatus == commentStatus) : 0,
                                 CommentScore = comment != null ? blog.Comments.Where(x => x.CommentStatus == commentStatus).Average(x => x.BlogScore) : 0,
                                 CommentStatus = comment == null ? false : comment.CommentStatus
                             };

            var query = leftQuery.Union(rightQuery).OrderByDescending(x => x.BlogID).AsQueryable();

            query = filter != null ? query.Where(filter) : query;

            query = take != 0 ? query.Skip(skip).Take(take) : query;

            return await query.ToListAsync();
        }

        public async Task<List<BlogCategoryandCommentCountDto>> GetListWithCategoryandCommentCountByPagingAsync(Expression<Func<BlogCategoryandCommentCountDto, bool>> filter = null, bool commentStatus = true, int take = 0, int page = 1)
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

            return AddNullObject<BlogCategoryandCommentCountDto>.GetListByPaging(await GetBlogListWithCategoryandCommentCountAsync(filter, commentStatus, take, skip), take, page, count);
        }

        public async Task<List<Blog>> GetListBlogWithCategoryAsync(Expression<Func<Blog, bool>> filter = null, int take = 0, int skip = 0)
        {
            var query = Context.Blogs.Include(x => x.Category).AsQueryable();

            query = filter != null ?
                query.Where(filter)
                : query;

            query = query.OrderByDescending(x => x.BlogID)
                    .Skip(skip)
                    .Take(take);

            return await query.ToListAsync();
        }

        public async Task<List<Blog>> GetListWithCategoryByPagingAsync(Expression<Func<Blog, bool>> filter = null, int take = 0, int page = 1)
        {
            int skip = 0;
            if (page > 1)
            {
                skip = take * (page - 1);
            }

            int count = await GetCountAsync(filter);

            if (skip >= count)
            {
                skip = 0;
                page = 1;
            }

            return AddNullObject<Blog>.GetListByPaging(await GetListBlogWithCategoryAsync(filter, take, skip), take, page, count);
        }

        public async Task<BlogCategoryandCommentCountandWriterDto> GetBlogWithCommentandWriterAsync(bool isCommentStatus, Expression<Func<BlogCategoryandCommentCountandWriterDto, bool>> filter)
        {
            try
            {
                var leftQuery = from blog in Context.Blogs
                                join category in Context.Categories
                                    on blog.CategoryID equals category.CategoryID
                                join writer in Context.Users
                                    on blog.WriterID equals writer.Id
                                join comment in Context.Comments.Where(x => x.CommentStatus == isCommentStatus)
                                    on blog.BlogID equals comment.BlogID into BlogComments
                                from comment in BlogComments.DefaultIfEmpty()
                                select new BlogCategoryandCommentCountandWriterDto
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
                                    CategoryStatus = category.CategoryStatus,
                                    CommentCount = comment != null ? blog.Comments.Count(x => x.CommentStatus == isCommentStatus) : 0,
                                    CommentScore = comment != null ? blog.Comments.Where(x => x.CommentStatus == isCommentStatus).Average(x => x.BlogScore) : 0,
                                    CommentStatus = comment == null ? false : comment.CommentStatus,
                                    WriterID = writer.Id,
                                    WriterNameSurName = writer.NameSurname,
                                    WriterUserName = writer.UserName,
                                };

                var rightQuery = from comment in Context.Comments.Where(x => x.CommentStatus == isCommentStatus)
                                 join category in Context.Categories
                                     on comment.Blog.CategoryID equals category.CategoryID
                                 join blog in Context.Blogs
                                     on comment.BlogID equals blog.BlogID into BlogComments
                                 from blog in BlogComments.DefaultIfEmpty()
                                 join writer in Context.Users
                                    on blog.WriterID equals writer.Id
                                 select new BlogCategoryandCommentCountandWriterDto
                                 {
                                     BlogID = blog.BlogID,
                                     BlogContent = blog.BlogContent,
                                     BlogImage = blog.BlogImage,
                                     BlogThumbnailImage = blog.BlogThumbnailImage,
                                     BlogTitle = blog.BlogTitle,
                                     BlogCreateDate = blog.BlogCreateDate,
                                     BlogStatus = blog.BlogStatus,
                                     CategoryID = category.CategoryID,
                                     CategoryName = category.CategoryName,
                                     CategoryStatus = category.CategoryStatus,
                                     CommentCount = comment != null ? blog.Comments.Count(x => x.CommentStatus == isCommentStatus) : 0,
                                     CommentScore = comment != null ? blog.Comments.Where(x => x.CommentStatus == isCommentStatus).Average(x => x.BlogScore) : 0,
                                     CommentStatus = comment == null ? false : comment.CommentStatus,
                                     WriterID = writer.Id,
                                     WriterNameSurName = writer.NameSurname,
                                     WriterUserName = writer.UserName
                                 };

                return await leftQuery.Union(rightQuery).Where(filter).FirstAsync();

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
