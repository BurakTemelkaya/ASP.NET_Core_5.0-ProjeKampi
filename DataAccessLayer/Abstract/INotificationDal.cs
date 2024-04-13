using CoreLayer.DataAccess;
using EntityLayer.Concrete;

namespace DataAccessLayer.Abstract
{
    public interface INotificationDal : IEntityRepository<Notification>
    {
    }
}
