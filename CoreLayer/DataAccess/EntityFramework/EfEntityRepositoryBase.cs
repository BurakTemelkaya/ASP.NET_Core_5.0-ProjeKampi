using CoreLayer.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.DataAccess.EntityFramework
{
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

        public async Task<TEntity> GetByIDAsync(int id)
        {
            var result = await _context.Set<TEntity>().FindAsync(id);
            return result;
        }

        public async Task<List<TEntity>> GetListAllAsync(Expression<Func<TEntity, bool>> filter = null, int take = 0, int skip = 0)
        {
            if (take > 0)
            {
                if (skip == 0)
                {
                    var count = filter == null ?
                    await GetCountAsync() : await GetCountAsync(filter);
                    skip = count - take;
                }
                return filter == null ?
                await _context.Set<TEntity>().Skip(skip).Take(take).ToListAsync() :
                await _context.Set<TEntity>().Where(filter).Skip(skip).Take(take).ToListAsync();
            }

            return filter == null ?
                await _context.Set<TEntity>().ToListAsync() :
                await _context.Set<TEntity>().Where(filter).ToListAsync();
        }

        public async Task<List<TEntity>> GetListAllByPagingAsync(Expression<Func<TEntity, bool>> filter = null, int take = 0, int page = 1)
        {
            var values = new List<TEntity>();

            var count = filter == null ?
                await GetCountAsync() : await GetCountAsync(filter);

            int skip = count - take;
            if (page > 1)
            {
                skip = count - (take * (page - 1));
                values.AddRange(AddNullObject<TEntity>.GetNullValuesForBefore(page, take));
            }

            var result = await GetListAllAsync(filter, take, skip);
            values.AddRange(result);

            values.AddRange(AddNullObject<TEntity>.GetNullValuesForAfter(page, take, count));
            return values;
        }

        public async Task InsertAsync(TEntity t)
        {
            await _context.AddAsync(t);
            await _context.SaveChangesAsync();
        }

        public async Task InsertRangeAsync(List<TEntity> t)
        {
            await _context.AddRangeAsync(t);
            await _context.SaveChangesAsync();
        }

        public async Task<TEntity> GetByFilterAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            if (filter == null)
                return await _context.Set<TEntity>().FirstOrDefaultAsync();
            else
                return await _context.Set<TEntity>().FirstOrDefaultAsync(filter);
        }

        public async Task UpdateAsync(TEntity t)
        {
            _context.Update(t);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(List<TEntity> t)
        {
            _context.UpdateRange(t);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            if (filter == null)
                return await _context.Set<TEntity>().CountAsync();
            else
                return await _context.Set<TEntity>().Where(filter).CountAsync();
        }
    }
}
