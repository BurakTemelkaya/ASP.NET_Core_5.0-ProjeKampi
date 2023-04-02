using CoreLayer.DataAccess;
using EntityLayer.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface ICommentDal : IEntityRepository<Comment>
    {
        Task<List<Comment>> GetListWithCommentByBlogAsync();
    }
}
