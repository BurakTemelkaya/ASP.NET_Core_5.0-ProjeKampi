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

        public async Task<List<Category>> GetListWithCategoryByBlog(Expression<Func<Category, bool>> filter = null)
        {
            return filter == null ?
                await Context.Categories.Include(x => x.Blogs).ToListAsync() :
                await Context.Categories.Include(x => x.Blogs).Where(filter).ToListAsync();
        }
    }
}
