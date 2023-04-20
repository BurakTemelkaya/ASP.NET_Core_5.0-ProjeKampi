using BusinessLayer.Abstract;
using CoreLayer.Utilities.Results;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class LogManager : ILogService
    {
        private readonly ILogDal _logDal;

        public LogManager(ILogDal logDal)
        {
            _logDal = logDal;
        }

        public async Task<IDataResult<Log>> GetLogByIdAsync(int id)
        {
            return new SuccessDataResult<Log>(await _logDal.GetByIDAsync(id));
        }

        public async Task<IDataResult<List<Log>>> GetLogListAsync(int take, int page, Expression<Func<Log, bool>> filter = null)
        {
            return new SuccessDataResult<List<Log>>(await _logDal.GetListAllByPagingAsync(filter, take, page));
        }
    }
}
