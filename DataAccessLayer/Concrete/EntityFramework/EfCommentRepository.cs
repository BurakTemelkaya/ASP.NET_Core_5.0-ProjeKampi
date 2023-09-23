using CoreLayer.DataAccess;
using CoreLayer.DataAccess.EntityFramework;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.EntityFramework
{
    public class EfCommentRepository : EfEntityRepositoryBase<Comment>, ICommentDal
    {
        public EfCommentRepository(Context context) : base(context)
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

        public async Task<List<Comment>> GetListWithCommentByBlogAsync(Expression<Func<Comment, bool>> filter = null, int take = 0, int skip = 0)
        {
            if (take > 0)
            {
                return filter == null ?
                    await Context.Comments.Include(x => x.Blog).OrderByDescending(x => x.CommentID).Skip(skip).Take(take).ToListAsync() :
                    await Context.Comments.Include(x => x.Blog).OrderByDescending(x => x.CommentID).Where(filter).Skip(skip).Take(take).ToListAsync();
            }
            return filter == null ?
                await Context.Comments.Include(x => x.Blog).OrderByDescending(x => x.CommentID).ToListAsync() :
                await Context.Comments.Include(x => x.Blog).OrderByDescending(x => x.CommentID).Where(filter).ToListAsync();
        }

        public async Task<List<Comment>> GetListWithCommentByBlogandPagingAsync(Expression<Func<Comment, bool>> filter = null, int take = 0, int page = 1)
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

            return AddNullObject<Comment>.GetListByPaging(await GetListWithCommentByBlogAsync(filter, take, skip), take, page, count);
        }

        public async Task<Comment> GetCommentByBlog(Expression<Func<Comment, bool>> filter = null)
        {
            return filter == null ?
                await Context.Comments.Include(x => x.Blog).FirstAsync() :
                await Context.Comments.Include(x => x.Blog).FirstAsync(filter);
        }
    }
}
