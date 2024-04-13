using CoreLayer.DataAccess.EntityFramework;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;

namespace DataAccessLayer.Concrete.EntityFramework
{
    public class EfNewsLetterRepository : EfEntityRepositoryBase<NewsLetter>, INewsLetterDal
    {
        public EfNewsLetterRepository(Context context) : base(context)
        {

        }
    }
}
