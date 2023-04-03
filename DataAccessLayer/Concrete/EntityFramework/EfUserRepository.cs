using CoreLayer.DataAccess.EntityFramework;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;

namespace DataAccessLayer.Concrete.EntityFramework
{
    public class EfUserRepository : EfEntityRepositoryBase<AppUser>, IUserDal
    {
        public EfUserRepository(Context context) : base(context)
        {

        }
    }
}
