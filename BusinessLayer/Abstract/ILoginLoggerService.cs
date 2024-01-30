using CoreLayer.Utilities.Results;
using EntityLayer.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface ILoginLoggerService
    {
        Task<IResultObject> AddAsync(string userName);
        Task<IDataResult<LoginLogger>> GetByUserAsync(int id);
        Task<IDataResult<List<LoginLogger>>> GetListByUserAsync(int page = 1, int take = 10);
        Task<IDataResult<LoginLogger>> GetAsync(int id);
        Task<IDataResult<List<LoginLogger>>> GetListAllAsync(int page = 1, int take = 10, string userName = null);
    }
}
