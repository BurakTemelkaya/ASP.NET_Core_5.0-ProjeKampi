using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class GenericRepository<T> : IGenericDal<T> where T : class
    {
        private readonly DbContextOptions<Context> _context;

        public GenericRepository(DbContextOptions<Context> context)
        {
            _context = context;
        }

        public async Task DeleteAsync(T t)
        {
            using var c = new Context(_context);
            c.Remove(t);
            await c.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(List<T> t)
        {
            using var c = new Context(_context);
            c.RemoveRange(t);
            await c.SaveChangesAsync();
        }

        public async Task<T> GetByIDAsync(int id)
        {
            using var c = new Context(_context);
            return await c.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetListAllAsync(Expression<Func<T, bool>> filter = null)
        {
            using var c = new Context(_context);
            return filter == null ?
                await c.Set<T>().ToListAsync() ://null ise
                await c.Set<T>().Where(filter).ToListAsync();//null değilse
        }

        public async Task InsertAsync(T t)
        {
            using var c = new Context(_context);
            await c.AddAsync(t);
            await c.SaveChangesAsync();
        }

        public async Task InsertRangeAsync(List<T> t)
        {
            using var c = new Context(_context);
            await c.AddRangeAsync(t);
            await c.SaveChangesAsync();
        }

        public async Task<T> GetByFilterAsync(Expression<Func<T, bool>> filter = null)
        {
            using var c = new Context(_context);
            if (filter == null)
                return await c.Set<T>().FirstOrDefaultAsync();
            else
                return await c.Set<T>().FirstOrDefaultAsync(filter);
        }

        public async Task UpdateAsync(T t)
        {
            using var c = new Context(_context);
            c.Update(t);
            await c.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(List<T> t)
        {
            using var c = new Context(_context);
            c.UpdateRange(t);
            await c.SaveChangesAsync();
        }

        public async Task<int> GetCountAsync(Expression<Func<T, bool>> filter = null)
        {
            using var c = new Context(_context);
            if (filter == null)
                return await c.Set<T>().CountAsync();
            else
                return await c.Set<T>().Where(filter).CountAsync();
        }
    }
}
