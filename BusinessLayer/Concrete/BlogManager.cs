using BusinessLayer.Abstract;
using BusinessLayer.ValidationRules;
using CoreLayer.Aspects.AutoFac.Validation;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CoreLayer.Utilities.FileUtilities;

namespace BusinessLayer.Concrete
{
    public class BlogManager : IBlogService
    {
        private readonly IBlogDal _blogDal;
        private readonly IBusinessUserService _userService;

        public BlogManager(IBlogDal blogDal, IBusinessUserService userService)
        {
            _blogDal = blogDal;
            _userService = userService;
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
        [ValidationAspect(typeof(BlogValidator))]
        public void TAdd(Blog t)
        {
            t.BlogCreateDate = DateTime.Now;
            _blogDal.Insert(t);
        }

        public void TDelete(Blog t)
        {
            _blogDal.Delete(t);
        }
        [ValidationAspect(typeof(BlogValidator))]
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

        public async Task<Blog> BlogAdd(Blog blog, string userName, IFormFile blogImage, IFormFile blogThumbnailImage)
        {
            var user = await _userService.FindByUserNameAsync(userName);
            if (user == null)
                return blog;
            if (blogImage != null && blogThumbnailImage != null)
            {
                blog.BlogImage = FileManager.FileAdd(blogImage, FileManager.StaticProfileImageLocation());
                blog.BlogThumbnailImage = FileManager.FileAdd(blogThumbnailImage, FileManager.StaticProfileImageLocation());
            }
            else if (blog.BlogImage == null || blog.BlogThumbnailImage == null)
                return blog;
            blog.WriterID = user.Id;
            blog.BlogCreateDate = DateTime.Now;
            _blogDal.Insert(blog);
            return blog;
        }
    }
}
