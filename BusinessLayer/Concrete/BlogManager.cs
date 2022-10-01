using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class BlogManager : IBlogService
    {
        private readonly IBlogDal _blogDal;

        public BlogManager(IBlogDal blogDal)
        {
            _blogDal = blogDal;
        }

        public List<Blog> GetBlogListWithCategory()
        {
            return _blogDal.GetListWithCategory();
        }
        public List<Blog> GetListWithCategoryByWriterBm(int id)
        {
            return _blogDal.GetListWithCategoryByWriter(id);
        }
        public Blog TGetByID(int id)
        {
            return _blogDal.GetByID(id);
        }
        public Blog GetBlogByID(int id)
        {
            return _blogDal.GetByID(id);
        }
        public List<Blog> GetList(Expression<Func<Blog, bool>> filter)
        {
            return _blogDal.GetListAll(filter);
        }

        public List<Blog> GetLastBlog(int count)
        {
            return _blogDal.GetListAll().TakeLast(count).ToList();
        }

        public List<Blog> GetBlogByWriter(int id)
        {
            return _blogDal.GetListAll(x => x.WriterID == id);
        }

        public void TAdd(Blog t)
        {
            _blogDal.Insert(t);
        }

        public void TDelete(Blog t)
        {
            _blogDal.Delete(t);
        }

        public void TUpdate(Blog t)
        {
            _blogDal.Update(t);
        }

        public Blog TGetByFilter(Expression<Func<Blog, bool>> filter)
        {
            return _blogDal.GetByFilter(filter);
        }

        public int GetCount(Expression<Func<Blog, bool>> filter = null)
        {
            return _blogDal.GetCount(filter);
        }
    }
}
