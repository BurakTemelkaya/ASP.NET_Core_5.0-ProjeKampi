using CoreLayer.Utilities.Results;
using EntityLayer.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract;

public interface INotificationService
{
    Task<IResultObject> TAddAsync(Notification t);
    Task<IResultObject> TDeleteAsync(Notification t);
    Task<IResultObject> TUpdateAsync(Notification t);
    Task<IDataResult<List<Notification>>> GetListAsync(bool? status = null, int take = 0, int skip = 0);
    Task<IDataResult<Notification>> TGetByIDAsync(int id);
    Task<IDataResult<int>> GetCountAsync();
    Task<IDataResult<List<Notification>>> GetListByTakeAsync(int take, bool? status = null);
}
