using CoreLayer.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IGenericService<T>
    {
        Task<IResult> TAddAsync(T t);
        Task<IResult> TDeleteAsync(T t);
        Task<IResult> TUpdateAsync(T t);
        Task<IDataResult<List<T>>> GetListAsync(Expression<Func<T, bool>> filter = null);
        Task<IDataResult<T>> TGetByIDAsync(int id);
        Task<IDataResult<T>> TGetByFilterAsync(Expression<Func<T, bool>> filter = null);
        Task<IDataResult<int>> GetCountAsync(Expression<Func<T, bool>> filter = null);
    }
}
