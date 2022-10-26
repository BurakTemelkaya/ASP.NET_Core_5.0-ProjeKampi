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
    public interface IBlogService : IGenericService<Blog>
    {
        Task<Blog> GetBlogByIDAsync(int id);
        Task<List<Blog>> GetListWithCategoryByWriterBmAsync(int id);
        Task<List<Blog>> GetBlogListWithCategoryAsync();
        Task<List<Blog>> GetBlogByWriterAsync(int id);
        Task<List<Blog>> GetLastBlogAsync(int count);
        Task<Blog> BlogAddAsync(Blog blog, string userName, IFormFile blogImage = null, IFormFile blogThumbnailImage = null);
    }
}
