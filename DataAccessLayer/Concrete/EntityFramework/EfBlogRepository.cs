using CoreLayer.DataAccess;
using CoreLayer.DataAccess.EntityFramework;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.EntityFramework;

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
        var query = from blog in Context.Blogs
                    join category in Context.Categories on blog.CategoryID equals category.CategoryID
                    join comment in Context.Comments.Where(x => x.CommentStatus == commentStatus) on blog.BlogID equals comment.BlogID into commentGroup
                    from subComment in commentGroup.DefaultIfEmpty()
                    join blogView in Context.BlogViews on blog.BlogID equals blogView.BlogId into blogViewGroup
                    from subBlogView in blogViewGroup.DefaultIfEmpty()
                    group new { blog, category, subComment, subBlogView } by new
                    {
                        blog.BlogID,
                        blog.BlogContent,
                        blog.BlogImage,
                        blog.BlogThumbnailImage,
                        blog.BlogTitle,
                        blog.BlogCreateDate,
                        blog.BlogStatus,
                        category.CategoryID,
                        category.CategoryName,
                        category.CategoryStatus
                    } into g
                    select new BlogCategoryandCommentCountDto
                    {
                        BlogID = g.Key.BlogID,
                        BlogContent = g.Key.BlogContent,
                        BlogImage = g.Key.BlogImage,
                        BlogThumbnailImage = g.Key.BlogThumbnailImage,
                        BlogTitle = g.Key.BlogTitle,
                        BlogCreateDate = g.Key.BlogCreateDate,
                        BlogStatus = g.Key.BlogStatus,
                        CategoryID = g.Key.CategoryID,
                        CategoryName = g.Key.CategoryName,
                        CategoryStatus = g.Key.CategoryStatus,
                        CommentCount = g.Count(x => x.subComment != null),
                        CommentScore = g.Where(x => x.subComment != null).Average(x => x.subComment != null ? (double?)x.subComment.BlogScore : 0) ?? 0,
                        BlogViewCount = g.Count(x => x.subBlogView != null),
                        CommentStatus = g.Any(x => x.subComment != null)
                    };

        query = query.OrderByDescending(x => x.BlogID);

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (take > 0)
        {
            query = query.Skip(skip).Take(take);
        }

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
        var stopWatch = Stopwatch.StartNew();

        var query = from blog in Context.Blogs
                    join category in Context.Categories
                        on blog.CategoryID equals category.CategoryID
                    join writer in Context.Users
                        on blog.WriterID equals writer.Id
                    join blogView in Context.BlogViews
                        on blog.BlogID equals blogView.BlogId into BlogViews
                    from blogView in BlogViews.DefaultIfEmpty()
                    let commentCount = Context.Comments.Count(x => x.BlogID == blog.BlogID && x.CommentStatus == isCommentStatus)
                    let commentScore = Context.Comments.Where(x => x.BlogID == blog.BlogID && x.CommentStatus == isCommentStatus).Average(x => (double?)x.BlogScore) ?? 0
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
                        CommentCount = commentCount,
                        CommentScore = commentScore,
                        CommentStatus = isCommentStatus,
                        WriterID = writer.Id,
                        WriterNameSurName = writer.NameSurname,
                        WriterUserName = writer.UserName,
                        BlogViewCount = blog.BlogViews.Count()
                    };

        var data = await query.Where(filter).FirstOrDefaultAsync();

        stopWatch.Stop();

        var time = stopWatch.ElapsedMilliseconds;

        return data;
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
