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

        }

        private Context Context
        {
            get
            {
                return _context as Context;
            }
        }

        public async Task<List<Comment>> GetListWithCommentByBlogAsync(int take = 0, int skip = 0)
        {
            if (take > 0)
            {
                return await Context.Comments.Include(x => x.Blog).OrderByDescending(x => x.CommentID).Skip(skip).Take(take).ToListAsync();
            }
            return await Context.Comments.Include(x => x.Blog).OrderByDescending(x => x.CommentID).ToListAsync();
        }

        public async Task<List<Comment>> GetListWithCommentByBlogandPagingAsync(int take = 0, int page = 1)
        {
            int skip = 0;
            if (page > 1)
            {
                skip = take * (page - 1);
            }

            int count = await GetCountAsync();

            return AddNullObject<Comment>.GetListByPaging(await GetListWithCommentByBlogAsync(take, skip), take, page, count);
        }
    }
}
