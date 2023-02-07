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
    public class EfMessageRepository : GenericRepository<Message>, IMessageDal
    {
        DbContextOptions<Context> _context;
        public EfMessageRepository(DbContextOptions<Context> context) : base(context)
        {
            _context= context;
        }

        public async Task<List<Message>> GetInboxWithMessageListAsync(int id, Expression<Func<Message, bool>> filter = null)
        {
            using var c = new Context(_context);
            return filter == null ?
                await c.Messages.Include(x => x.SenderUser)
            .Where(x => x.ReceiverUser.Id == id).ToListAsync() :
           await c.Messages.Include(x => x.SenderUser)
            .Where(x => x.ReceiverUser.Id == id).Where(filter).ToListAsync();
        }
        public async Task<Message> GetReceivedMessageAsync(int id, Expression<Func<Message, bool>> filter = null)
        {
            using var c = new Context(_context);
            return filter == null ?
                await c.Messages.Include(x => x.SenderUser)
            .Where(x => x.ReceiverUser.Id == id).FirstOrDefaultAsync() :
            await c.Messages.Include(x => x.SenderUser)
            .Where(x => x.ReceiverUser.Id == id).FirstOrDefaultAsync(filter);
        }
        public async Task<Message> GetSendedMessageAsync(int id, Expression<Func<Message, bool>> filter = null)
        {
            using var c = new Context(_context);
            return filter == null ?
               await c.Messages.Include(x => x.ReceiverUser)
            .Where(x => x.SenderUser.Id == id).FirstOrDefaultAsync() :
            await c.Messages.Include(x => x.ReceiverUser)
            .Where(x => x.SenderUser.Id == id).FirstOrDefaultAsync(filter);
        }

        public async Task<List<Message>> GetSendBoxWithMessageListAsync(int id, Expression<Func<Message, bool>> filter = null)
        {
            using var c = new Context(_context);
            return filter == null ?
               await c.Messages.Include(x => x.ReceiverUser)
            .Where(x => x.SenderUser.Id == id).ToListAsync() :
            await c.Messages.Include(x => x.ReceiverUser)
            .Where(x => x.SenderUser.Id == id).Where(filter).ToListAsync();
        }
    }
}
