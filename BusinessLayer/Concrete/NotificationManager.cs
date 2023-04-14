using BusinessLayer.Abstract;
using CoreLayer.Aspects.AutoFac.Caching;
using CoreLayer.Utilities.Results;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class NotificationManager : INotificationService
    {
        private readonly INotificationDal _notificationDal;

        public NotificationManager(INotificationDal notificationDal)
        {
            _notificationDal = notificationDal;
        }

        [CacheAspect]
        public async Task<IDataResult<int>> GetCountAsync(Expression<Func<Notification, bool>> filter = null)
        {
            return new SuccessDataResult<int>(await _notificationDal.GetCountAsync(filter));
        }

        [CacheAspect]
        public async Task<IDataResult<List<Notification>>> GetListAsync(Expression<Func<Notification, bool>> filter = null)
        {
            return new SuccessDataResult<List<Notification>>(await _notificationDal.GetListAllAsync(filter));
        }

        public async Task<IDataResult<List<Notification>>> GetListByTakeAsync(int take, Expression<Func<Notification, bool>> filter = null)
        {
            return new SuccessDataResult<List<Notification>>(await _notificationDal.GetListAllAsync(filter, take));
        }

        [CacheRemoveAspect("INotificationService.Get")]
        public async Task<IResult> TAddAsync(Notification t)
        {
            await _notificationDal.InsertAsync(t);
            return new SuccessResult();
        }

        [CacheRemoveAspect("INotificationService.Get")]
        public async Task<IResult> TDeleteAsync(Notification t)
        {
            await _notificationDal.DeleteAsync(t);
            return new SuccessResult();
        }

        [CacheAspect]
        public async Task<IDataResult<Notification>> TGetByFilterAsync(Expression<Func<Notification, bool>> filter = null)
        {
            return new SuccessDataResult<Notification>(await _notificationDal.GetByFilterAsync(filter));
        }

        [CacheAspect]
        public async Task<IDataResult<Notification>> TGetByIDAsync(int id)
        {
            return new SuccessDataResult<Notification>(await _notificationDal.GetByIDAsync(id));
        }

        [CacheRemoveAspect("INotificationService.Get")]
        public async Task<IResult> TUpdateAsync(Notification t)
        {
            await _notificationDal.UpdateAsync(t);
            return new SuccessResult();
        }
    }
}
