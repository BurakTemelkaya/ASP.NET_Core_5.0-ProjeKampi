using CoreLayer.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using X.PagedList;

namespace CoreLayer.DataAccess;

public interface IEntityRepository<TEntity> where TEntity : class, IEntity, new()
{
    Task<TEntity> InsertAsync(TEntity t,CancellationToken cancellationToken=default);
    Task<List<TEntity>> InsertRangeAsync(List<TEntity> t, CancellationToken cancellationToken = default);
    Task DeleteAsync(TEntity t, CancellationToken cancellationToken = default);
    Task DeleteRangeAsync(List<TEntity> t, CancellationToken cancellationToken = default);
    Task<TEntity> UpdateAsync(TEntity t, CancellationToken cancellationToken = default);
    Task<List<TEntity>> UpdateRangeAsync(List<TEntity> t, CancellationToken cancellationToken = default);
    Task<List<TEntity>> GetListAllAsync(Expression<Func<TEntity, bool>> filter = null, int take = 0, int skip = 0, bool sortInReverse = true, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool enableTracking = false, CancellationToken cancellationToken = default);
    Task<List<TEntity>> GetListAllByPagingAsync(Expression<Func<TEntity, bool>> filter = null, int take = 0, int page = 1, bool sortInReverse = true, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool enableTracking = false, CancellationToken cancellationToken = default);
    Task<TEntity> GetByIDAsync(int id, bool enableTracking = false, CancellationToken cancellationToken = default);
    Task<TEntity> GetByFilterAsync(Expression<Func<TEntity, bool>> filter = null, bool enableTracking = false, CancellationToken cancellationToken = default);
    Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter = null, bool enableTracking = false, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<Dictionary<DateTime, int>> GetChartDataAsync(
        string dateSelector,
        TimeUnit timeUnit,
        DateTime? startDate = null,
        DateTime? endDate = null,
        Expression<Func<TEntity, bool>> predicate = null, CancellationToken cancellationToken = default);
    Task<IPagedList<TEntity>> GetPagedListAsync(int pageNumber,int pageSize,Expression<Func<TEntity, bool>> predicate = null,
    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, bool enableTracking = false, CancellationToken cancellationToken = default);
}
