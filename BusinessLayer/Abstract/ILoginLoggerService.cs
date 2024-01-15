using CoreLayer.Utilities.Results;
using EntityLayer.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface ILoginLoggerService
    {
        Task<IResultObject> AddAsync(string userName);
        Task<IDataResult<LoginLogger>> GetAsync(int id);
        Task<IDataResult<List<LoginLogger>>> GetListAsync(int page = 0, int take = 0);
    }
}
