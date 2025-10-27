using CoreLayer.DataAccess.EntityFramework;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.EntityFramework
{
    public class EfMessageDraftRepository : EfEntityRepositoryBase<MessageDraft>, IMessageDraftDal
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EfMessageDraftRepository(Context context,IHttpContextAccessor httpContextAccessor) : base(context)
        {
            Context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _httpContextAccessor = httpContextAccessor;
        }

        private Context Context => _context as Context;

        private CancellationToken CancellationToken => _httpContextAccessor.HttpContext?.RequestAborted ?? CancellationToken.None;

        public async Task<List<MessageDraft>> GetMessageDraftListAsync(Expression<Func<MessageDraft, bool>> filter = null)
        {
            return filter == null ?
            await Context.MessagesDrafts.Include(x => x.User).ToListAsync(CancellationToken) :
            await Context.MessagesDrafts.Include(x => x.User).Where(filter).ToListAsync(CancellationToken);
        }
        public async Task<List<MessageDraft>> GetMessageDraftListByUserIdAsync(int id, Expression<Func<MessageDraft, bool>> filter = null)
        {
            return filter == null ?
                await Context.MessagesDrafts.Include(x => x.User).Where(x => x.UserId == id).ToListAsync(CancellationToken) :
                await Context.MessagesDrafts.Include(x => x.User).Where(x => x.UserId == id).Where(filter).ToListAsync(CancellationToken);
        }
        public async Task<MessageDraft> GetMessageDraftByUserIdAsync(int id, Expression<Func<MessageDraft, bool>> filter = null)
        {
            return filter == null ?
                await Context.MessagesDrafts.Include(x => x.User).Where(x => x.UserId == id).FirstOrDefaultAsync(CancellationToken) :
                await Context.MessagesDrafts.Include(x => x.User).Where(x => x.UserId == id).Where(filter).FirstOrDefaultAsync(CancellationToken);
        }
    }
}
