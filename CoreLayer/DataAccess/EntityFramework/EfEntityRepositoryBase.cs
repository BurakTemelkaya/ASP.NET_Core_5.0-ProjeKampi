using CoreLayer.Entities;
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
            var result =await _context.Set<TEntity>().FindAsync(id);
            _context.Entry(result).State = EntityState.Detached;
            return result;
        }

        public async Task<List<TEntity>> GetListAllAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return filter == null ?
                await _context.Set<TEntity>().AsNoTracking().ToListAsync() :
                await _context.Set<TEntity>().AsNoTracking().Where(filter).ToListAsync();
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
                return await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync();
            else
                return await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(filter);
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
                return await _context.Set<TEntity>().AsNoTracking().CountAsync();
            else
                return await _context.Set<TEntity>().AsNoTracking().Where(filter).CountAsync();
        }
    }
}
