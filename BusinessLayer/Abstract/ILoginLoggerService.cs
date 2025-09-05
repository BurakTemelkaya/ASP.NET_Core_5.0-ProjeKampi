using CoreLayer.Utilities.Results;
using EntityLayer.Concrete;
using System.Threading.Tasks;
using X.PagedList;

namespace BusinessLayer.Abstract;

public interface ILoginLoggerService
{
    Task<IResultObject> AddAsync(string userName);
    Task<IDataResult<LoginLogger>> GetByUserAsync(int id);
    Task<IDataResult<IPagedList<LoginLogger>>> GetListByUserAsync(int page = 1, int take = 10);
    Task<IDataResult<LoginLogger>> GetAsync(int id);
    Task<IDataResult<IPagedList<LoginLogger>>> GetListAllAsync(int page = 1, int take = 10, string userName = null);
}
