using BusinessLayer.Models;
using CoreLayer.Utilities.Results;
using EntityLayer.Concrete;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface INewsLetterService : IGenericService<NewsLetter>
    {
        Task<IDataResult<NewsLetter>> GetByMailAsync(string mail);
        Task<IResultObject> SendMailAsync(NewsLetterSendMailsModel model, Expression<Func<NewsLetter, bool>> filter = null);
    }
}
