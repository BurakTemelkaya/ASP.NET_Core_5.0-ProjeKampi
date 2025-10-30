using CoreLayer.DataAccess.EntityFramework;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;

namespace DataAccessLayer.Concrete.EntityFramework;

public class EfLoginLoggerRepository : EfEntityRepositoryBase<LoginLogger>, ILoginLoggerDal
{

    public EfLoginLoggerRepository(Context context) : base(context)
    {

    }

}
