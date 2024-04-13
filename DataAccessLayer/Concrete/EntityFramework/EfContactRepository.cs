using CoreLayer.DataAccess.EntityFramework;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;

namespace DataAccessLayer.Concrete.EntityFramework
{
    public class EfContactRepository : EfEntityRepositoryBase<Contact>, IContactDal
    {
        public EfContactRepository(Context context) : base(context)
        {

        }
    }
}
