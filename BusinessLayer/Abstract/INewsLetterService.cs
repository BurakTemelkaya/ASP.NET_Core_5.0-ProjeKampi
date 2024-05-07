using BusinessLayer.Models;
using CoreLayer.Utilities.Results;
using EntityLayer.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface INewsLetterService : IGenericService<NewsLetter>
    {
        Task<IDataResult<NewsLetter>> GetByMailAsync(string mail);
        Task<IResultObject> SendMailAsync(NewsLetterSendMailsModel model, bool mailStatus);
        Task<IDataResult<int>> GetCountAsync();
        Task<IDataResult<List<NewsLetter>>> GetListAsync();
    }
}
