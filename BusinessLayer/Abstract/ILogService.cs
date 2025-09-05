using CoreLayer.Utilities.Results;
using EntityLayer.Concrete;
using System.Threading.Tasks;
using X.PagedList;

namespace BusinessLayer.Abstract;

public interface ILogService
{
    public Task<IResultObject> AddLogAsync(string audit, string message);
    public Task<IDataResult<IPagedList<Log>>> GetLogListAsync(int take, int page, string search = null);
    public Task<IDataResult<Log>> GetLogByIdAsync(int id);
}
