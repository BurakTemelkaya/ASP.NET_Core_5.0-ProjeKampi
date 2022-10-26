using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface INewsLetterService : IGenericService<NewsLetter>
    {
        Task AddNewsLetterAsync(NewsLetter newsLetter);
        Task<NewsLetter> GetByMailAsync(string mail);
    }
}
