using AutoMapper.Configuration;
using BusinessLayer.Abstract;
using BusinessLayer.Constants;
using BusinessLayer.Models;
using BusinessLayer.ValidationRules;
using CoreLayer.Aspects.AutoFac.Validation;
using CoreLayer.Utilities.MailUtilities;
using CoreLayer.Utilities.Results;
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
    public class NewsLetterManager : INewsLetterService
    {
        private readonly INewsLetterDal _newsLetterDal;
        readonly IMailService _mailService;

        public NewsLetterManager(INewsLetterDal newsLetterDal, IMailService mailService)
        {
            _newsLetterDal = newsLetterDal;
            _mailService = mailService;
        }

        public async Task<IDataResult<NewsLetter>> GetByMailAsync(string mail)
        {
            return new SuccessDataResult<NewsLetter>(await _newsLetterDal.GetByFilterAsync(x => x.Mail == mail));
        }

        public async Task<IDataResult<int>> GetCountAsync(Expression<Func<NewsLetter, bool>> filter = null)
        {
            return new SuccessDataResult<int>(await _newsLetterDal.GetCountAsync(filter));
        }

        public async Task<IDataResult<List<NewsLetter>>> GetListAsync(Expression<Func<NewsLetter, bool>> filter = null)
        {
            return new SuccessDataResult<List<NewsLetter>>(await _newsLetterDal.GetListAllAsync(filter));
        }

        public async Task<IResultObject> SendMailAsync(NewsLetterSendMailsModel model, Expression<Func<NewsLetter, bool>> filter = null)
        {
            var value = await _newsLetterDal.GetListAllAsync(filter);
            bool isSend = _mailService.SendMails(value.Select(x => x.Mail).ToArray(), model.Subject, model.Content);
            if (isSend)
                return new SuccessResult();
            else
                return new ErrorResult(Messages.NewsLetterNotSending);
        }

        [ValidationAspect(typeof(NewsLetterValidator))]
        public async Task<IResultObject> TAddAsync(NewsLetter t)
        {
            var mail = await GetByMailAsync(t.Mail);

            if (mail.Data == null)
            {
                t.MailStatus = true;
                await _newsLetterDal.InsertAsync(t);
                return new SuccessResult();
            }

            return new ErrorResult(Messages.NewsLetterAlreadyRegistered);
        }

        public async Task<IResultObject> TDeleteAsync(NewsLetter t)
        {
            await _newsLetterDal.DeleteAsync(t);
            return new SuccessResult();
        }

        public async Task<IDataResult<NewsLetter>> TGetByFilterAsync(Expression<Func<NewsLetter, bool>> filter = null)
        {
            return new SuccessDataResult<NewsLetter>(await _newsLetterDal.GetByFilterAsync(filter));
        }

        public async Task<IDataResult<NewsLetter>> TGetByIDAsync(int id)
        {
            return new SuccessDataResult<NewsLetter>(await _newsLetterDal.GetByIDAsync(id));
        }

        [ValidationAspect(typeof(NewsLetterValidator))]
        public async Task<IResultObject> TUpdateAsync(NewsLetter t)
        {
            await _newsLetterDal.UpdateAsync(t);
            return new SuccessResult();
        }
    }
}
