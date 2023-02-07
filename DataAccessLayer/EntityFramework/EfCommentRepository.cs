using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.Repositories;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.EntityFramework
{
    public class EfCommentRepository : GenericRepository<Comment>, ICommentDal
    {
        private readonly DbContextOptions<Context> _context;
        public EfCommentRepository(DbContextOptions<Context> context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Comment>> GetListWithCommentByBlogAsync()
        {
            using var c = new Context(_context);
            return await c.Comments.Include(x => x.Blog).ToListAsync();
        }
    }
}
