using CoreLayer.Utilities.Results;
using EntityLayer.Concrete;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface INewsLetterDraftService : IGenericService<NewsLetterDraft>
    {
        Task<IResultObject> DeleteById(int id);
    }
}
