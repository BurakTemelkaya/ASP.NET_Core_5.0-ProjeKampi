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

        public async Task<List<Blog>> GetBlogListWithCategoryAsync()
        {
            return await _blogDal.GetListWithCategoryAsync();
        }
        public async Task<List<Blog>> GetListWithCategoryByWriterBmAsync(int id)
        {
            return await _blogDal.GetListWithCategoryByWriterAsync(id);
        }
        public async Task<Blog> TGetByIDAsync(int id)
        {
            return await _blogDal.GetByIDAsync(id);
        }
        public async Task<Blog> GetBlogByIDAsync(int id)
        {
            return await _blogDal.GetByIDAsync(id);
        }
        public async Task<List<Blog>> GetListAsync(Expression<Func<Blog, bool>> filter)
        {
            return await _blogDal.GetListAllAsync(filter);
        }

        public async Task<List<Blog>> GetLastBlogAsync(int count)
        {
            var value = await _blogDal.GetListAllAsync();
            return value.TakeLast(count).ToList();
        }

        public async Task<List<Blog>> GetBlogByWriterAsync(int id)
        {
            return await _blogDal.GetListAllAsync(x => x.WriterID == id);
        }
        [ValidationAspect(typeof(BlogValidator))]
        public async Task TAddAsync(Blog t)
        {
            t.BlogCreateDate = DateTime.Now;
            await _blogDal.InsertAsync(t);
        }

        public async Task TDeleteAsync(Blog t)
        {
            await _blogDal.DeleteAsync(t);
        }
        [ValidationAspect(typeof(BlogValidator))]
        public async Task TUpdateAsync(Blog t)
        {
            await _blogDal.UpdateAsync(t);
        }

        public async Task<Blog> TGetByFilterAsync(Expression<Func<Blog, bool>> filter)
        {
            return await _blogDal.GetByFilterAsync(filter);
        }

        public async Task<int> GetCountAsync(Expression<Func<Blog, bool>> filter = null)
        {
            return await _blogDal.GetCountAsync(filter);
        }

        public async Task<Blog> BlogAddAsync(Blog blog, string userName, IFormFile blogImage, IFormFile blogThumbnailImage)
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
            await _blogDal.InsertAsync(blog);
            return blog;
        }
    }
}
