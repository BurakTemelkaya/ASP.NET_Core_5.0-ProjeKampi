using BusinessLayer.Abstract;
using BusinessLayer.ValidationRules;
using CoreLayer.Aspects.AutoFac.Validation;
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

        public NewsLetterManager(INewsLetterDal newsLetterDal)
        {
            _newsLetterDal = newsLetterDal;
        }
        [ValidationAspect(typeof(NewsLetterValidator))]
        public void AddNewsLetter(NewsLetter newsLetter)
        {
            newsLetter.MailStatus = true;
            _newsLetterDal.Insert(newsLetter);
        }

        public NewsLetter GetByMail(string mail)
        {
            return _newsLetterDal.GetByFilter(x => x.Mail == mail);
        }

        public int GetCount(Expression<Func<NewsLetter, bool>> filter = null)
        {
            return _newsLetterDal.GetCount(filter);
        }

        public List<NewsLetter> GetList(Expression<Func<NewsLetter, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public void TAdd(NewsLetter t)
        {
            throw new NotImplementedException();
        }

        public void TDelete(NewsLetter t)
        {
            throw new NotImplementedException();
        }

        public NewsLetter TGetByFilter(Expression<Func<NewsLetter, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public NewsLetter TGetByID(int id)
        {
            throw new NotImplementedException();
        }

        public void TUpdate(NewsLetter t)
        {
            throw new NotImplementedException();
        }
    }
}
