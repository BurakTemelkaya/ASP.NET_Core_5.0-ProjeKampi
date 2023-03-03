using BusinessLayer.Abstract;
using BusinessLayer.ValidationRules;
using CoreLayer.Aspects.AutoFac.Validation;
using CoreLayer.Utilities.FileUtilities;
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
        public NewsLetterDraftManager(INewsLetterDraftDal newsLetterDraftDal)
        {
            _newsLetterDraftDal = newsLetterDraftDal;
        }

        public async Task<bool> DeleteById(int id)
        {
            var value = await _newsLetterDraftDal.GetByIDAsync(id);
            if (value != null)
            {
                await _newsLetterDraftDal.DeleteAsync(value);
                return true;
            }
            return false;
            
        }

        public async Task<int> GetCountAsync(Expression<Func<NewsLetterDraft, bool>> filter = null)
        {
            return await _newsLetterDraftDal.GetCountAsync(filter);
        }

        public async Task<List<NewsLetterDraft>> GetListAsync(Expression<Func<NewsLetterDraft, bool>> filter = null)
        {
            var values = await _newsLetterDraftDal.GetListAllAsync(filter);
            foreach (var item in values)
                item.Content = await TextFileManager.ReadTextFileAsync(item.Content, 30);
            return values;
        }

        
        public async Task TAddAsync(NewsLetterDraft t)
        {
            t.TimeToAdd = DateTime.Now;
            t.Content = await TextFileManager.TextFileAddAsync(t.Content, TextFileManager.GetNewsLetterDraftContentFileLocation());
            await _newsLetterDraftDal.InsertAsync(t);
        }

        public async Task TDeleteAsync(NewsLetterDraft t)
        {
            DeleteFileManager.DeleteFile(t.Content);
            await _newsLetterDraftDal.DeleteAsync(t);
        }

        public async Task<NewsLetterDraft> TGetByFilterAsync(Expression<Func<NewsLetterDraft, bool>> filter = null)
        {
            var value = await _newsLetterDraftDal.GetByFilterAsync(filter);
            value.Content = await TextFileManager.ReadTextFileAsync(value.Content);
            return value;
        }

        public async Task<NewsLetterDraft> TGetByIDAsync(int id)
        {
            var value = await _newsLetterDraftDal.GetByIDAsync(id);
            value.Content = await TextFileManager.ReadTextFileAsync(value.Content);
            return value;
        }

        public async Task TUpdateAsync(NewsLetterDraft t)
        {
            if (await TextFileManager.ReadTextFileAsync(t.Content) != t.Content)
            {
                DeleteFileManager.DeleteFile(t.Content);
                t.Content = await TextFileManager.TextFileAddAsync(t.Content, TextFileManager.GetNewsLetterDraftContentFileLocation());
            }
            else
            {
                var oldValue = await TGetByIDAsync(t.NewsLetterDraftId);
                t.Content = oldValue.Content;
            }
            if (t.NewsLetterDraftId!=0)
            {
                await _newsLetterDraftDal.UpdateAsync(t);
            }            
        }
    }
}
