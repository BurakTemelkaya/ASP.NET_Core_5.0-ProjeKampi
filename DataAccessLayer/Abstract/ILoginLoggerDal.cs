using CoreLayer.DataAccess;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface ILoginLoggerDal : IEntityRepository<LoginLogger>
    {
        Task<List<LoginLogger>> GetLogginLoggerListByUserAsync(Expression<Func<LoginLogger, bool>> filter = null, int take = 0, int skip = 0);

        Task<List<LoginLogger>> GetLogginLoggerListByUserandPagingAsync(Expression<Func<LoginLogger, bool>> filter = null, int take = 0, int skip = 0);
    }
}
