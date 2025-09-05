using CoreLayer.Utilities.Results;
using EntityLayer.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

namespace BusinessLayer.Abstract;

public interface INotificationService
{
    Task<IResultObject> TAddAsync(Notification t);
    Task<IResultObject> TDeleteAsync(Notification t);
    Task<IResultObject> TUpdateAsync(Notification t);
    Task<IDataResult<IPagedList<Notification>>> GetListAsync(bool? status = null, int pageNumber = 1, int pageSize = 10);
    Task<IDataResult<Notification>> TGetByIDAsync(int id);
    Task<IDataResult<int>> GetCountAsync();
    Task<IDataResult<List<Notification>>> GetListByTakeAsync(int take, bool? status = null);
}
