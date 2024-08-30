using CoreLayer.DataAccess.EntityFramework;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;

namespace DataAccessLayer.Concrete.EntityFramework;

public class EfUserSessionRepository : EfEntityRepositoryBase<UserSession>, IUserSessionDal
{
    public EfUserSessionRepository(Context context) : base(context)
    {
        
    }
}
