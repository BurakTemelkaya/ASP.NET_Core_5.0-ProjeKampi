using CoreLayer.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IGenericService<T>
    {
        Task<IResultObject> TAddAsync(T t);
        Task<IResultObject> TDeleteAsync(T t);
        Task<IResultObject> TUpdateAsync(T t);
        Task<IDataResult<List<T>>> GetListAsync(Expression<Func<T, bool>> filter = null);
        Task<IDataResult<T>> TGetByIDAsync(int id);
        Task<IDataResult<T>> TGetByFilterAsync(Expression<Func<T, bool>> filter = null);
        Task<IDataResult<int>> GetCountAsync(Expression<Func<T, bool>> filter = null);
    }
}
