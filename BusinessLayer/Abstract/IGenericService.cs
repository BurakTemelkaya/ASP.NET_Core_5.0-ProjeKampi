using CoreLayer.Utilities.Results;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IGenericService<T>
    {
        Task<IResultObject> TAddAsync(T t);
        Task<IResultObject> TDeleteAsync(T t);
        Task<IResultObject> TUpdateAsync(T t);
        Task<IDataResult<T>> TGetByIDAsync(int id);
    }
}
