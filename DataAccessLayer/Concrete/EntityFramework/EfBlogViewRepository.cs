using CoreLayer.DataAccess.EntityFramework;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;

namespace DataAccessLayer.Concrete.EntityFramework;

public class EfBlogViewRepository : EfEntityRepositoryBase<BlogView>, IBlogViewDal
{
    public EfBlogViewRepository(Context context):base(context)
    {
        
    }
}
