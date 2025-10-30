using CoreLayer.DataAccess.EntityFramework;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;

namespace DataAccessLayer.Concrete.EntityFramework;

public class EfAboutRepository : EfEntityRepositoryBase<About>, IAboutDal
{
    public EfAboutRepository(Context context) : base(context)
    {

    }
}