using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class MessageDraftManager : IMessageDraftService
    {
        readonly IMessageDraftDal _messageDraftDal;
        public MessageDraftManager(IMessageDraftDal messageDraftDal)
        {
            _messageDraftDal = messageDraftDal;
        }

        public async Task<int> GetCountAsync(Expression<Func<MessageDraft, bool>> filter = null)
        {
            return await _messageDraftDal.GetCountAsync(filter);
        }

        public async Task<List<MessageDraft>> GetListAsync(Expression<Func<MessageDraft, bool>> filter = null)
        {
            return await _messageDraftDal.GetListAllAsync(filter);
        }

        public async Task TAddAsync(MessageDraft t)
        {
            await _messageDraftDal.InsertAsync(t);
        }

        public async Task TDeleteAsync(MessageDraft t)
        {
            await _messageDraftDal.DeleteAsync(t);
        }

        public async Task<MessageDraft> TGetByFilterAsync(Expression<Func<MessageDraft, bool>> filter = null)
        {
            return await _messageDraftDal.GetByFilterAsync(filter);
        }

        public async Task<MessageDraft> TGetByIDAsync(int id)
        {
            return await _messageDraftDal.GetByIDAsync(id);
        }

        public async Task TUpdateAsync(MessageDraft t)
        {
            await _messageDraftDal.UpdateAsync(t);
        }
    }
}
