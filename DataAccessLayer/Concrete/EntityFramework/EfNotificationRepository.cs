using CoreLayer.DataAccess.EntityFramework;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;

namespace DataAccessLayer.Concrete.EntityFramework;

public class EfNotificationRepository : EfEntityRepositoryBase<Notification>, INotificationDal
{
    public EfNotificationRepository(Context context) : base(context)
    {

    }
}