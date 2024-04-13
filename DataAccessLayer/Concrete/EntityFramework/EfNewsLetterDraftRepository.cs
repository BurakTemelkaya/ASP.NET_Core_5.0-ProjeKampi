using CoreLayer.DataAccess.EntityFramework;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;

namespace DataAccessLayer.Concrete.EntityFramework
{
    public class EfNewsLetterDraftRepository : EfEntityRepositoryBase<NewsLetterDraft>, INewsLetterDraftDal
    {
        public EfNewsLetterDraftRepository(Context context) : base(context)
        {

        }
    }
}
