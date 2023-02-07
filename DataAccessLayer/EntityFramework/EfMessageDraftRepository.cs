using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.Repositories;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.EntityFramework
{
    public class EfMessageDraftRepository : GenericRepository<MessageDraft>, IMessageDraftDal
    {
        private readonly DbContextOptions<Context> _context;
        public EfMessageDraftRepository(DbContextOptions<Context> context) : base(context)
        {
            _context = context;
        }

        public async Task<List<MessageDraft>> GetMessageDraftListAsync(Expression<Func<MessageDraft, bool>> filter = null)
        {
            using var c = new Context(_context);
            return filter == null ?
            await c.MessagesDrafts.Include(x => x.User).ToListAsync() :
            await c.MessagesDrafts.Include(x => x.User).Where(filter).ToListAsync();
        }
        public async Task<List<MessageDraft>> GetMessageDraftListByUserIdAsync(int id, Expression<Func<MessageDraft, bool>> filter = null)
        {
            using var c = new Context(_context);
            return filter == null ?
                await c.MessagesDrafts.Include(x => x.User).Where(x => x.UserId == id).ToListAsync() :
                await c.MessagesDrafts.Include(x => x.User).Where(x => x.UserId == id).Where(filter).ToListAsync();
        }
        public async Task<MessageDraft> GetMessageDraftByUserIdAsync(int id, Expression<Func<MessageDraft, bool>> filter = null)
        {
            using var c = new Context(_context);
            return filter == null ?
                await c.MessagesDrafts.Include(x => x.User).Where(x => x.UserId == id).FirstOrDefaultAsync() :
                await c.MessagesDrafts.Include(x => x.User).Where(x => x.UserId == id).Where(filter).FirstOrDefaultAsync();
        }
    }
}
