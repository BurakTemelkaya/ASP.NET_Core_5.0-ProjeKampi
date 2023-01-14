using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IGenericDal<T> where T : class
    {
        Task InsertAsync(T t);
        Task InsertRangeAsync(List<T> t);
        Task DeleteAsync(T t);
        Task DeleteRangeAsync(List<T> t);
        Task UpdateAsync(T t);
        Task UpdateRangeAsync(List<T> t);
        Task<List<T>> GetListAllAsync(Expression<Func<T, bool>> filter = null);
        Task<T> GetByIDAsync(int id);
        Task<T> GetByFilterAsync(Expression<Func<T, bool>> filter = null);
        Task<int> GetCountAsync(Expression<Func<T, bool>> filter = null);
    }
}
