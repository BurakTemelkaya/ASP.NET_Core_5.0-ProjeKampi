using CoreLayer.Utilities.Results;
using EntityLayer.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface INewsLetterDraftService : IGenericService<NewsLetterDraft>
    {
        Task<IResultObject> DeleteById(int id);
        Task<IDataResult<List<NewsLetterDraft>>> GetListAsync();
    }
}
