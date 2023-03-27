using CoreDemo.Models;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IBlogService
    {
        Task<Blog> GetBlogByIDAsync(int id);
        Task<Blog> GetBlogByIdForUpdate(int id);
        Task<List<BlogandCommentCount>> GetBlogListByMainPage(string id, int page = 1, string search = null);
        Task<List<Blog>> GetListWithCategoryByWriterBmAsync(string userName, Expression<Func<Blog, bool>> filter = null);
        Task<List<Blog>> GetBlogListWithCategoryAsync(Expression<Func<Blog, bool>> filter = null);
        Task<List<Blog>> GetBlogListByWriterAsync(int id);
        Task<List<Blog>> GetLastBlogAsync(int count);
        Task<Blog> BlogAddAsync(Blog blog, string userName, IFormFile blogImage = null, IFormFile blogThumbnailImage = null);
        Task<Blog> BlogUpdateAsync(Blog blog, string userName, IFormFile blogImage = null, IFormFile blogThumbnailImage = null);
        Task DeleteBlogAsync(Blog blog, string userName);
        Task DeleteBlogByAdminAsync(Blog blog);
        Task<int> GetCountAsync(Expression<Func<Blog, bool>> filter = null);
        Task<List<Blog>> GetListAsync(Expression<Func<Blog, bool>> filter = null);
        Task ChangedBlogStatusAsync(int id, string userName);
        Task ChangedBlogStatusByAdminAsync(int id);
        Task<Blog> BlogAdminUpdateAsync(Blog blog, IFormFile blogImage = null, IFormFile blogThumbnailImage = null);
        Task<Blog> GetFileNameContentBlogByIDAsync(int id);
    }
}
