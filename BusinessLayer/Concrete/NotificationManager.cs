using BusinessLayer.Abstract;
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

        public async Task<int> GetCountAsync(Expression<Func<Notification, bool>> filter = null)
        {
            return await _notificationDal.GetCountAsync(filter);
        }

        public async Task<List<Notification>> GetListAsync(Expression<Func<Notification, bool>> filter = null)
        {
            return await _notificationDal.GetListAllAsync(filter);
        }

        public async Task TAddAsync(Notification t)
        {
            await _notificationDal.InsertAsync(t);
        }

        public async Task TDeleteAsync(Notification t)
        {
            await _notificationDal.DeleteAsync(t);
        }

        public async Task<Notification> TGetByFilterAsync(Expression<Func<Notification, bool>> filter = null)
        {
            return await _notificationDal.GetByFilterAsync(filter);
        }

        public async Task<Notification> TGetByIDAsync(int id)
        {
            return await _notificationDal.GetByIDAsync(id);
        }

        public async Task TUpdateAsync(Notification t)
        {
            await _notificationDal.UpdateAsync(t);
        }
    }
}
