using BusinessLayer.Abstract;
using CoreLayer.Aspects.AutoFac.Caching;
using CoreLayer.Utilities.Results;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

namespace BusinessLayer.Concrete;

public class NotificationManager : INotificationService
{
    private readonly INotificationDal _notificationDal;

    public NotificationManager(INotificationDal notificationDal)
    {
        _notificationDal = notificationDal;
    }

    [CacheAspect]
    public async Task<IDataResult<int>> GetCountAsync()
    {
        return new SuccessDataResult<int>(await _notificationDal.GetCountAsync());
    }

    [CacheAspect]
    public async Task<IDataResult<IPagedList<Notification>>> GetListAsync(bool? status = null, int pageNumber = 1, int pageSize = 10)
    {
        IPagedList<Notification> data = await _notificationDal.GetPagedListAsync(pageNumber, pageSize, status == null
            ? null : x => x.NotificationStatus == status);

        return new SuccessDataResult<IPagedList<Notification>>(data);
    }

    public async Task<IDataResult<List<Notification>>> GetListByTakeAsync(int take, bool? status = null)
    {
        return new SuccessDataResult<List<Notification>>(status == null
            ? await _notificationDal.GetListAllAsync(null, take)
            : await _notificationDal.GetListAllAsync(x => x.NotificationStatus == status, take));
    }

    [CacheRemoveAspect("INotificationService.Get")]
    public async Task<IResultObject> TAddAsync(Notification t)
    {
        await _notificationDal.InsertAsync(t);
        return new SuccessResult();
    }

    [CacheRemoveAspect("INotificationService.Get")]
    public async Task<IResultObject> TDeleteAsync(Notification t)
    {
        await _notificationDal.DeleteAsync(t);
        return new SuccessResult();
    }

    [CacheAspect]
    public async Task<IDataResult<Notification>> TGetByIDAsync(int id)
    {
        return new SuccessDataResult<Notification>(await _notificationDal.GetByIDAsync(id));
    }

    [CacheRemoveAspect("INotificationService.Get")]
    public async Task<IResultObject> TUpdateAsync(Notification t)
    {
        await _notificationDal.UpdateAsync(t);
        return new SuccessResult();
    }
}
