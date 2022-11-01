using BusinessLayer.Models;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface INewsLetterService : IGenericService<NewsLetter>
    {
        Task<NewsLetter> GetByMailAsync(string mail);
        Task<bool> SendMailAsync(NewsLetterSendMailsModel model, Expression<Func<NewsLetter, bool>> filter = null);
    }
}
