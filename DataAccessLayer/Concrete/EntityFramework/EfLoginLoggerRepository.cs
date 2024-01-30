using CoreLayer.DataAccess;
using CoreLayer.DataAccess.EntityFramework;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.EntityFramework
{
    public class EfLoginLoggerRepository : EfEntityRepositoryBase<LoginLogger>, ILoginLoggerDal
    {
        public EfLoginLoggerRepository(Context context) : base(context)
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

        public async Task<List<LoginLogger>> GetLogginLoggerListByUserandPagingAsync(Expression<Func<LoginLogger, bool>> filter = null, int take = 0, int skip = 0)
        {
            if (take > 0)
            {
                return filter == null ?
                    await Context.LogginLoggers.Include(x => x.User).OrderByDescending(x => x.Id).Skip(skip).Take(take).ToListAsync() :
                    await Context.LogginLoggers.Include(x => x.User).OrderByDescending(x => x.Id).Where(filter).Skip(skip).Take(take).ToListAsync();
            }
            return filter == null ?
                await Context.LogginLoggers.Include(x => x.User).OrderByDescending(x => x.Id).ToListAsync() :
                await Context.LogginLoggers.Include(x => x.User).OrderByDescending(x => x.Id).Where(filter).ToListAsync();
        }

        public async Task<List<LoginLogger>> GetLogginLoggerListByUserAsync(Expression<Func<LoginLogger, bool>> filter = null, int take = 0, int page = 1)
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

            return AddNullObject<LoginLogger>.GetListByPaging(await GetLogginLoggerListByUserandPagingAsync(filter, take, skip), take, page, count);
        }
    }
}
