using AutoMapper.Configuration;
using BusinessLayer.Abstract;
using BusinessLayer.Models;
using BusinessLayer.ValidationRules;
using CoreLayer.Aspects.AutoFac.Validation;
using CoreLayer.Utilities.MailUtilities;
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

        public async Task<NewsLetter> GetByMailAsync(string mail)
        {
            return await _newsLetterDal.GetByFilterAsync(x => x.Mail == mail);
        }

        public async Task<int> GetCountAsync(Expression<Func<NewsLetter, bool>> filter = null)
        {
            return await _newsLetterDal.GetCountAsync(filter);
        }

        public async Task<List<NewsLetter>> GetListAsync(Expression<Func<NewsLetter, bool>> filter = null)
        {
            return await _newsLetterDal.GetListAllAsync(filter);
        }

        public async Task<bool> SendMailAsync(NewsLetterSendMailsModel model, Expression<Func<NewsLetter, bool>> filter = null)
        {
            var value = await _newsLetterDal.GetListAllAsync(filter);
            bool isSend = _mailService.SendMails(value.Select(x => x.Mail).ToArray(), model.Subject, model.Content);
            if (isSend)
                return true;
            else
                return false;
        }

        [ValidationAspect(typeof(NewsLetterValidator))]
        public async Task TAddAsync(NewsLetter t)
        {
            t.MailStatus = true;
            await _newsLetterDal.InsertAsync(t);
        }

        public async Task TDeleteAsync(NewsLetter t)
        {
            await _newsLetterDal.DeleteAsync(t);
        }

        public async Task<NewsLetter> TGetByFilterAsync(Expression<Func<NewsLetter, bool>> filter = null)
        {
            return await _newsLetterDal.GetByFilterAsync(filter);
        }

        public async Task<NewsLetter> TGetByIDAsync(int id)
        {
            return await _newsLetterDal.GetByIDAsync(id);
        }

        [ValidationAspect(typeof(NewsLetterValidator))]
        public async Task TUpdateAsync(NewsLetter t)
        {
            await _newsLetterDal.UpdateAsync(t);
        }
    }
}
