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
        Task TAddAsync(T t);
        Task TDeleteAsync(T t);
        Task TUpdateAsync(T t);
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> filter = null);
        Task<T> TGetByIDAsync(int id);
        Task<T> TGetByFilterAsync(Expression<Func<T, bool>> filter = null);
        Task<int> GetCountAsync(Expression<Func<T, bool>> filter = null);
    }
}
