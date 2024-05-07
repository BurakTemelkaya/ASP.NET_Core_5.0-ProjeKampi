using BusinessLayer.Abstract;
using BusinessLayer.Constants;
using CoreLayer.Utilities.Business;
using CoreLayer.Utilities.FileUtilities;
using CoreLayer.Utilities.Results;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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

        public async Task<IResultObject> DeleteById(int id)
        {
            IResultObject result = BusinessRules.Run(NewsLetterDraftIdNotEqualZero(id));

            if (!result.Success)
            {
                return result;
            }

            var value = await _newsLetterDraftDal.GetByIDAsync(id);
            if (value != null)
            {
                await _newsLetterDraftDal.DeleteAsync(value);
                return new SuccessResult();
            }

            return new ErrorResult(Messages.NewsLetterDraftNotFound);

        }

        public async Task<IDataResult<int>> GetCountAsync(Expression<Func<NewsLetterDraft, bool>> filter = null)
        {
            return new SuccessDataResult<int>(await _newsLetterDraftDal.GetCountAsync(filter));
        }

        public async Task<IDataResult<List<NewsLetterDraft>>> GetListAsync()
        {
            var values = await _newsLetterDraftDal.GetListAllAsync();
            foreach (var item in values)
                item.Content = await TextFileManager.ReadTextFileAsync(item.Content, 30);
            return new SuccessDataResult<List<NewsLetterDraft>>(values);
        }


        public async Task<IResultObject> TAddAsync(NewsLetterDraft t)
        {
            t.TimeToAdd = DateTime.Now;
            t.Content = await TextFileManager.TextFileAddAsync(t.Content, ContentFileLocations.GetNewsLetterDraftContentFileLocation());
            await _newsLetterDraftDal.InsertAsync(t);
            return new SuccessResult();
        }

        public async Task<IResultObject> TDeleteAsync(NewsLetterDraft t)
        {
            DeleteFileManager.DeleteFile(t.Content);
            await _newsLetterDraftDal.DeleteAsync(t);
            return new SuccessResult();
        }

        public async Task<IDataResult<NewsLetterDraft>> TGetByFilterAsync(Expression<Func<NewsLetterDraft, bool>> filter = null)
        {
            var value = await _newsLetterDraftDal.GetByFilterAsync(filter);
            value.Content = await TextFileManager.ReadTextFileAsync(value.Content);
            return new SuccessDataResult<NewsLetterDraft>(value);
        }

        public async Task<IDataResult<NewsLetterDraft>> TGetByIDAsync(int id)
        {
            var value = await _newsLetterDraftDal.GetByIDAsync(id);

            if (value == null)
            {
                return new ErrorDataResult<NewsLetterDraft>(Messages.NewsLetterDraftNotFound);
            }

            value.Content = await TextFileManager.ReadTextFileAsync(value.Content);
            return new SuccessDataResult<NewsLetterDraft>(value);
        }

        public async Task<IResultObject> TUpdateAsync(NewsLetterDraft t)
        {
            IResultObject result = BusinessRules.Run(NewsLetterDraftIdNotEqualZero(t.NewsLetterDraftId));

            if (!result.Success)
            {
                return result;
            }

            if (await TextFileManager.ReadTextFileAsync(t.Content) != t.Content)
            {
                DeleteFileManager.DeleteFile(t.Content);
                t.Content = await TextFileManager.TextFileAddAsync(t.Content, ContentFileLocations.GetNewsLetterDraftContentFileLocation());
            }
            else
            {
                var oldValue = await TGetByIDAsync(t.NewsLetterDraftId);
                t.Content = oldValue.Data.Content;
            }

            await _newsLetterDraftDal.UpdateAsync(t);
            return new SuccessResult();

        }

        private IResultObject NewsLetterDraftIdNotEqualZero(int newsLetterId)
        {
            if (newsLetterId == 0)
            {
                return new ErrorResult(Messages.NewsLetterDraftNotEmpty);
            }
            return new SuccessResult();
        }
    }
}
