using CoreLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoreLayer.DataAccess.EntityFramework;

public class EfEntityRepositoryBase<TEntity> : IEntityRepository<TEntity>
    where TEntity : class, IEntity, new()
{

    protected readonly DbContext _context;

    public EfEntityRepositoryBase(DbContext context)
    {
        _context = context;
        _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public async Task DeleteAsync(TEntity t)
    {
        _context.Remove(t);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteRangeAsync(List<TEntity> t)
    {
        _context.RemoveRange(t);
        await _context.SaveChangesAsync();
    }

    public async Task<TEntity> GetByIDAsync(int id, bool enableTracking = false)
    {
        var result = await _context.Set<TEntity>().FindAsync(id);

        if (result != null && !enableTracking)
        {
            _context.Entry(result).State = EntityState.Detached;
        }

        return result;
    }

    public async Task<List<TEntity>> GetListAllAsync(Expression<Func<TEntity, bool>> filter = null, int take = 0, int skip = 0
        , bool sortInReverse = true, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool enableTracking = false)
    {
        var queryable = _context.Set<TEntity>().AsQueryable();

        if (take > 0)
        {
            if (sortInReverse)
            {
                var count = await GetCountAsync(filter);
                if (skip == 0)
                {
                    skip = count - take;
                }
                else if (skip > 0)
                {
                    skip = count - skip;
                }

                if (skip < 0)
                {
                    skip = 0;
                }
            }

            if (include != null) queryable = include(queryable);

            if (!enableTracking) queryable = queryable.AsNoTracking();

            var filteredResult = filter == null ?
            await queryable.Skip(skip).Take(take).ToListAsync() :
            await queryable.Where(filter).Skip(skip).Take(take).ToListAsync();

            return filteredResult;
        }

        if (include != null) queryable = include(queryable);

        var result = filter == null ?
            await queryable.ToListAsync() :
            await queryable.Where(filter).ToListAsync();

        if (sortInReverse)
        {
            result.Reverse();
        }

        return result;
    }

    public async Task<List<TEntity>> GetListAllByPagingAsync(Expression<Func<TEntity, bool>> filter = null, int take = 0, int page = 1
        , bool sortInReverse = true, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool enableTracking = false)
    {
        var values = new List<TEntity>();
        var count = await GetCountAsync(filter);
        int skip = count - (take * page);

        if (skip >= count)
        {
            skip = 0;
            page = 1;
        }

        values.AddRange(AddNullObject<TEntity>.GetNullValuesForBefore(page, take));

        int realTake = 0;

        if (skip < 0)
        {
            realTake += take + skip;
            skip = 0;
        }
        else
        {
            realTake = take;
        }

        var queryable = _context.Set<TEntity>().AsQueryable();

        if (include != null) queryable = include(queryable);

        if (!enableTracking) queryable = queryable.AsNoTracking();

        var result = filter == null ?
            await queryable.Skip(skip).Take(realTake).ToListAsync() :
            await queryable.Where(filter).Skip(skip).Take(realTake).ToListAsync();

        if (sortInReverse)
        {
            result.Reverse();
        }

        values.AddRange(result);

        values.AddRange(AddNullObject<TEntity>.GetNullValuesForAfter(page, take, count));

        return values;
    }

    public async Task<TEntity> InsertAsync(TEntity t)
    {
        await _context.AddAsync(t);
        await _context.SaveChangesAsync();

        return t;
    }

    public async Task<List<TEntity>> InsertRangeAsync(List<TEntity> t)
    {
        await _context.AddRangeAsync(t);
        await _context.SaveChangesAsync();

        return t;
    }

    public async Task<TEntity> GetByFilterAsync(Expression<Func<TEntity, bool>> filter = null, bool enableTracking = false)
    {
        var queryable = _context.Set<TEntity>().AsQueryable();

        if (filter != null) queryable = queryable.Where(filter);

        if (!enableTracking) queryable = queryable.AsNoTracking();

        return await queryable.FirstOrDefaultAsync();
    }

    public async Task<TEntity> UpdateAsync(TEntity t)
    {
        _context.Update(t);
        await _context.SaveChangesAsync();

        return t;
    }

    public async Task<List<TEntity>> UpdateRangeAsync(List<TEntity> t)
    {
        _context.UpdateRange(t);
        await _context.SaveChangesAsync();

        return t;
    }

    public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter = null, bool enableTracking = false)
    {
        var queryable = _context.Set<TEntity>().AsQueryable();

        if (filter != null) queryable = queryable.Where(filter);

        if (!enableTracking) queryable = queryable.AsNoTracking();

        return await queryable.CountAsync();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}