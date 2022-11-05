using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.Repositories;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.EntityFramework
{
    public class EfBlogRepository : GenericRepository<Blog>, IBlogDal
    {
        public async Task<List<Blog>> GetListWithCategoryAsync(Expression<Func<Blog, bool>> filter = null)
        {
            using var c = new Context();
            return filter == null ?
            await c.Blogs.Include(x => x.Category).ToListAsync() :
            await c.Blogs.Include(x => x.Category).Where(filter).ToListAsync();
        }
        public async Task<List<Blog>> GetListWithCategoryByWriterAsync(int id, Expression<Func<Blog, bool>> filter = null)
        {
            using var c = new Context();
            return filter == null ?
                await c.Blogs.Include(x => x.Category).Where(x => x.WriterID == id).ToListAsync() :
                await c.Blogs.Include(x => x.Category).Where(x => x.WriterID == id).Where(filter).ToListAsync();

        }
    }
}
