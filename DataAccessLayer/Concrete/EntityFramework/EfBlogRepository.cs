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

        public async Task<List<Blog>> GetListWithCategoryAsync(Expression<Func<Blog, bool>> filter = null)
        {

            if (take > 0 && page > 0)
            {
                var values = new List<Blog>();
                int skip = 0;
                if (page > 1)
                {
                    skip = take * (page - 1);
                    values.AddRange(AddNullObject<Blog>.GetNullValuesForBefore(page, take));
                }

                var result = filter == null ?
                    await Context.Blogs.Include(x => x.Category).Include(x => x.Comments).SkipLast(skip).TakeLast(take).ToListAsync() :
                    await Context.Blogs.Include(x => x.Category).Include(x => x.Comments).Where(filter).SkipLast(skip).TakeLast(take).ToListAsync();

                values.AddRange(result);

                var count = filter == null ?
                    await GetCountAsync() : await GetCountAsync(filter);

                values.AddRange(AddNullObject<Blog>.GetNullValuesForAfter(page, take, count));

                return values;
            }

            return filter == null ?
            await Context.Blogs.Include(x => x.Category).ToListAsync() :
            await Context.Blogs.Include(x => x.Category).Where(filter).ToListAsync();
        }

        public async Task<List<Blog>> GetListWithCategoryByWriterAsync(int id, Expression<Func<Blog, bool>> filter = null)
        {
            return filter == null ?
                await Context.Blogs.Include(x => x.Category).Where(x => x.WriterID == id).ToListAsync() :
                await Context.Blogs.Include(x => x.Category).Where(x => x.WriterID == id).Where(filter).ToListAsync();

        }
    }
}
