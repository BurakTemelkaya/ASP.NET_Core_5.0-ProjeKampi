using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IBlogService : IGenericService<Blog>
    {
        Blog GetBlogByID(int id);
        public List<Blog> GetListWithCategoryByWriterBm(int id);
        List<Blog> GetBlogListWithCategory();
        List<Blog> GetBlogByWriter(int id);
        List<Blog> GetLastBlog(int count);
    }
}
