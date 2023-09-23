using CoreLayer.DataAccess.EntityFramework;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.EntityFramework
{
    public class EfMessageRepository : EfEntityRepositoryBase<Message>, IMessageDal
    {
        public EfMessageRepository(Context context) : base(context)
        {
            Context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        private Context Context
        {
            get
            {
                return _context as Context;
            }
        }

        public async Task<List<Message>> GetInboxWithMessageListAsync(int id, Expression<Func<Message, bool>> filter = null, int take = 0, int skip = 0)
        {

            if (take > 0)
            {
                return filter == null ?
                await Context.Messages.Include(x => x.SenderUser).Where(x => x.ReceiverUser.Id == id).OrderByDescending(x => x.MessageID).Skip(skip).Take(take).ToListAsync() :
                await Context.Messages.Include(x => x.SenderUser).Where(x => x.ReceiverUser.Id == id).OrderByDescending(x => x.MessageID).Where(filter).Skip(skip).Take(take).ToListAsync();
            }
            return filter == null ?
                await Context.Messages.Include(x => x.SenderUser)
            .Where(x => x.ReceiverUser.Id == id).ToListAsync() :
           await Context.Messages.Include(x => x.SenderUser)
            .Where(x => x.ReceiverUser.Id == id).Where(filter).ToListAsync();
        }
        public async Task<Message> GetReceivedMessageAsync(int id, Expression<Func<Message, bool>> filter = null)
        {
            return filter == null ?
                await Context.Messages.Include(x => x.SenderUser)
            .Where(x => x.ReceiverUser.Id == id).FirstOrDefaultAsync() :
            await Context.Messages.Include(x => x.SenderUser)
            .Where(x => x.ReceiverUser.Id == id).FirstOrDefaultAsync(filter);
        }
        public async Task<Message> GetSendedMessageAsync(int id, Expression<Func<Message, bool>> filter = null)
        {
            return filter == null ?
               await Context.Messages.Include(x => x.ReceiverUser)
            .Where(x => x.SenderUser.Id == id).FirstOrDefaultAsync() :
            await Context.Messages.Include(x => x.ReceiverUser)
            .Where(x => x.SenderUser.Id == id).FirstOrDefaultAsync(filter);
        }

        public async Task<List<Message>> GetSendBoxWithMessageListAsync(int id, Expression<Func<Message, bool>> filter = null, int take = 0, int skip = 0)
        {
            if (take > 0)
            {
                return filter == null ?
                await Context.Messages
                .Include(x => x.ReceiverUser)
                .Where(x => x.SenderUser.Id == id)
                    .OrderByDescending(x => x.MessageID)
                        .Skip(skip)
                        .Take(take)
                            .ToListAsync() :
                await Context.Messages
                .Include(x => x.ReceiverUser)
                    .Where(x => x.SenderUser.Id == id)
                        .OrderByDescending(x => x.MessageID)
                            .Where(filter)
                                .Skip(skip)
                                .Take(take)
                                    .ToListAsync();
            }

            return filter == null ?
               await Context.Messages.Include(x => x.ReceiverUser)
            .Where(x => x.SenderUser.Id == id).ToListAsync() :
            await Context.Messages.Include(x => x.ReceiverUser)
            .Where(x => x.SenderUser.Id == id).Where(filter).ToListAsync();
        }
    }
}
