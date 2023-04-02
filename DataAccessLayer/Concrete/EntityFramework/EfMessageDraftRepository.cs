using CoreLayer.DataAccess.EntityFramework;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
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
    public class EfMessageDraftRepository : EfEntityRepositoryBase<MessageDraft>, IMessageDraftDal
    {
        public EfMessageDraftRepository(Context context) : base(context)
        {

        }

        private Context Context
        {
            get
            {
                return _context as Context;
            }
        }

        public async Task<List<MessageDraft>> GetMessageDraftListAsync(Expression<Func<MessageDraft, bool>> filter = null)
        {
            return filter == null ?
            await Context.MessagesDrafts.Include(x => x.User).ToListAsync() :
            await Context.MessagesDrafts.Include(x => x.User).Where(filter).ToListAsync();
        }
        public async Task<List<MessageDraft>> GetMessageDraftListByUserIdAsync(int id, Expression<Func<MessageDraft, bool>> filter = null)
        {
            return filter == null ?
                await Context.MessagesDrafts.Include(x => x.User).Where(x => x.UserId == id).ToListAsync() :
                await Context.MessagesDrafts.Include(x => x.User).Where(x => x.UserId == id).Where(filter).ToListAsync();
        }
        public async Task<MessageDraft> GetMessageDraftByUserIdAsync(int id, Expression<Func<MessageDraft, bool>> filter = null)
        {
            return filter == null ?
                await Context.MessagesDrafts.Include(x => x.User).Where(x => x.UserId == id).FirstOrDefaultAsync() :
                await Context.MessagesDrafts.Include(x => x.User).Where(x => x.UserId == id).Where(filter).FirstOrDefaultAsync();
        }
    }
}
