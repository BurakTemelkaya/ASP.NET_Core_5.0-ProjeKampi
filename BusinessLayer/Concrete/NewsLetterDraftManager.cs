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
    public class NewsLetterDraftManager : INewsLetterDraftService
    {
        readonly INewsLetterDraftDal _newsLetterDraftDal;
        readonly IBusinessUserService _userService;
        public NewsLetterDraftManager(INewsLetterDraftDal newsLetterDraftDal, IBusinessUserService userService)
        {
            _newsLetterDraftDal = newsLetterDraftDal;
            _userService = userService;
        }

        public async Task<int> GetCountAsync(Expression<Func<NewsLetterDraft, bool>> filter = null)
        {
            return await _newsLetterDraftDal.GetCountAsync(filter);
        }

        public async Task<List<NewsLetterDraft>> GetListAsync(Expression<Func<NewsLetterDraft, bool>> filter = null)
        {
            return await _newsLetterDraftDal.GetListAllAsync(filter);
        }

        public async Task TAddAsync(NewsLetterDraft t)
        {
            t.TimeToAdd = DateTime.Now;
            await _newsLetterDraftDal.InsertAsync(t);
        }

        public async Task TDeleteAsync(NewsLetterDraft t)
        {
            await _newsLetterDraftDal.DeleteAsync(t);
        }

        public async Task<NewsLetterDraft> TGetByFilterAsync(Expression<Func<NewsLetterDraft, bool>> filter = null)
        {
            return await _newsLetterDraftDal.GetByFilterAsync(filter);
        }

        public async Task<NewsLetterDraft> TGetByIDAsync(int id)
        {
            return await _newsLetterDraftDal.GetByIDAsync(id);
        }

        public async Task TUpdateAsync(NewsLetterDraft t)
        {
            await _newsLetterDraftDal.UpdateAsync(t);
        }
    }
}
