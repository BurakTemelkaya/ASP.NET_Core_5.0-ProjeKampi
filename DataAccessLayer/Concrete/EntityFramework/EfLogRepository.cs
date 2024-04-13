using CoreLayer.DataAccess.EntityFramework;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;

namespace DataAccessLayer.Concrete.EntityFramework
{
    public class EfLogRepository : EfEntityRepositoryBase<Log>, ILogDal
    {
        public EfLogRepository(Context context) : base(context)
        {

        }
    }
}
