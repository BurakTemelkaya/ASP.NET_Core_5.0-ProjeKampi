using CoreLayer.Utilities.Results;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface ILogService
    {
        public Task<IDataResult<List<Log>>> GetLogListAsync(int take, int page, Expression<Func<Log, bool>> filter = null);

        public Task<IDataResult<Log>> GetLogByIdAsync(int id);
    }
}
