using CoreLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.DataAccess
{
    public interface IEntityRepository<T> where T : class, IEntity, new()
    {
        Task InsertAsync(T t);
        Task InsertRangeAsync(List<T> t);
        Task DeleteAsync(T t);
        Task DeleteRangeAsync(List<T> t);
        Task UpdateAsync(T t);
        Task UpdateRangeAsync(List<T> t);
        Task<List<T>> GetListAllAsync(Expression<Func<T, bool>> filter = null, int take = 0, int skip = 0, bool sortInReverse = true);
        Task<List<T>> GetListAllByPagingAsync(Expression<Func<T, bool>> filter = null, int take = 0, int page = 1, bool sortInReverse = true);
        Task<T> GetByIDAsync(int id);
        Task<T> GetByFilterAsync(Expression<Func<T, bool>> filter = null);
        Task<int> GetCountAsync(Expression<Func<T, bool>> filter = null);
    }
}
