using CoreLayer.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoreLayer.DataAccess
{
    public interface IEntityRepository<TEntity> where TEntity : class, IEntity, new()
    {
        Task InsertAsync(TEntity t);
        Task InsertRangeAsync(List<TEntity> t);
        Task DeleteAsync(TEntity t);
        Task DeleteRangeAsync(List<TEntity> t);
        Task UpdateAsync(TEntity t);
        Task UpdateRangeAsync(List<TEntity> t);
        Task<List<TEntity>> GetListAllAsync(Expression<Func<TEntity, bool>> filter = null, int take = 0, int skip = 0, bool sortInReverse = true, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,bool enableTracking=false);
        Task<List<TEntity>> GetListAllByPagingAsync(Expression<Func<TEntity, bool>> filter = null, int take = 0, int page = 1, bool sortInReverse = true, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool enableTracking = false);
        Task<TEntity> GetByIDAsync(int id, bool enableTracking = false);
        Task<TEntity> GetByFilterAsync(Expression<Func<TEntity, bool>> filter = null, bool enableTracking = false);
        Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter = null, bool enableTracking = false);
        Task SaveChangesAsync();
    }
}
