using CoreLayer.DataAccess;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IBlogDal : IEntityRepository<Blog>
    {
        Task<List<Blog>> GetListWithCategoryandCommentAsync(Expression<Func<Blog, bool>> filter = null, int take = 0, int skip = 0);

        Task<List<Blog>> GetListWithCategoryandCommentByPaging(Expression<Func<Blog, bool>> filter = null, int take = 0, int page = 1);

        Task<List<Blog>> GetListWithCategoryByWriterAsync(int id, Expression<Func<Blog, bool>> filter = null);
    }
}
