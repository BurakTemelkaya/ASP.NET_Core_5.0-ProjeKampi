using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IBlogDal
    {
        List<Blog> ListAllBlog();
        void BlogAdd(Blog blog);
        void DeleteBlogy(Blog blog);
        void UpdateBlog(Blog blog);
        Blog GetByID(int id);
    }
}
