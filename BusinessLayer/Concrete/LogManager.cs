using BusinessLayer.Abstract;
using CoreLayer.Utilities.Results;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete;

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

    public async Task<IDataResult<List<Log>>> GetLogListAsync(int take, int page, string search = null)
    {
        var data = await _logDal.GetListAllByPagingAsync(
         x => (search == null || (x.Details.ToLower().Contains(search.ToLower()))), take, page);

        return new SuccessDataResult<List<Log>>(data);
    }
}