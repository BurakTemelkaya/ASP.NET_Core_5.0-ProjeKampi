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

        public async Task<List<Blog>> GetListWithCategoryandCommentAsync(Expression<Func<Blog, bool>> filter = null, int take = 0, int skip = 0)
        {
            if (take > 0)
            {
                return filter == null ?
                await Context.Blogs.Include(x => x.Category).Include(x => x.Comments).OrderByDescending(x => x.BlogID).Skip(skip).Take(take).ToListAsync() :
                await Context.Blogs.Include(x => x.Category).Include(x => x.Comments).OrderByDescending(x => x.BlogID).Where(filter).Skip(skip).Take(take).ToListAsync();
            }
            return filter == null ?
                await Context.Blogs.Include(x => x.Category).Include(x => x.Comments).ToListAsync() :
                await Context.Blogs.Include(x => x.Category).Include(x => x.Comments).Where(filter).ToListAsync();
        }

        public async Task<List<Blog>> GetListWithCategoryandCommentByPaging(Expression<Func<Blog, bool>> filter = null, int take = 0, int page = 1)
        {
            var values = new List<Blog>();
            int skip = 0;
            if (page > 1)
            {
                skip = take * (page - 1);
                values.AddRange(AddNullObject<Blog>.GetNullValuesForBefore(page, take));
            }

            values.AddRange(await GetListWithCategoryandCommentAsync(filter, take, skip));

            var count = filter == null ?
                await GetCountAsync() : await GetCountAsync(filter);

            values.AddRange(AddNullObject<Blog>.GetNullValuesForAfter(page, take, count));

            return values;
        }

        public async Task<List<Blog>> GetListWithCategoryByWriterAsync(int id, Expression<Func<Blog, bool>> filter = null)
        {
            return filter == null ?
                await Context.Blogs.Include(x => x.Category).Where(x => x.WriterID == id).ToListAsync() :
                await Context.Blogs.Include(x => x.Category).Where(x => x.WriterID == id).Where(filter).ToListAsync();

        }
    }
}
