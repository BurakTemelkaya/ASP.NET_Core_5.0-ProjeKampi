using CoreLayer.DataAccess.EntityFramework;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.EntityFramework
{
    public class EfCommentRepository : EfEntityRepositoryBase<Comment>, ICommentDal
    {
        public EfCommentRepository(Context context) : base(context)
        {

        }

        private Context Context
        {
            get
            {
                return _context as Context;
            }
        }

        public async Task<List<Comment>> GetListWithCommentByBlogAsync()
        {
            return await Context.Comments.Include(x => x.Blog).ToListAsync();
        }
    }
}
