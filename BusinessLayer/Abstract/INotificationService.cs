using CoreLayer.Utilities.Results;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface INotificationService
    {
        Task<IResultObject> TAddAsync(Notification t);
        Task<IResultObject> TDeleteAsync(Notification t);
        Task<IResultObject> TUpdateAsync(Notification t);
        Task<IDataResult<List<Notification>>> GetListAsync(Expression<Func<Notification, bool>> filter = null);
        Task<IDataResult<Notification>> TGetByIDAsync(int id);
        Task<IDataResult<Notification>> TGetByFilterAsync(Expression<Func<Notification, bool>> filter = null);
        Task<IDataResult<int>> GetCountAsync(Expression<Func<Notification, bool>> filter = null);

        Task<IDataResult<List<Notification>>> GetListByTakeAsync(int take, Expression<Func<Notification, bool>> filter = null);
    }
}
