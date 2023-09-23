using CoreDemo.Models;
using CoreLayer.Utilities.Results;
using EntityLayer.Concrete;
using EntityLayer.DTO;
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
        Task<IDataResult<Blog>> GetBlogByIDAsync(int id);
        Task<IDataResult<Blog>> GetBlogByIdForUpdate(int id);
        Task<IDataResult<List<Blog>>> GetListWithCategory(Expression<Func<Blog, bool>> filter = null, int take = 0, int skip = 0);
        Task<IDataResult<List<Blog>>> GetListWithCategoryByPaging(int take = 0, int page = 1, Expression<Func<Blog, bool>> filter = null);
        Task<IDataResult<List<Blog>>> GetListWithCategoryByWriterWitchPagingAsync(string userName, int take, int page);
        Task<IDataResult<List<BlogCategoryandCommentCountDto>>> GetBlogListWithCategoryandCommentCountAsync(int take = 0, Expression<Func<BlogCategoryandCommentCountDto, bool>> filter = null);
        Task<IDataResult<List<BlogCategoryandCommentCountDto>>> GetBlogListWithCategoryandCommentCountByPagingAsync(int take, int page, Expression<Func<BlogCategoryandCommentCountDto, bool>> filter = null);
        Task<IDataResult<List<Blog>>> GetBlogListByWriterAsync(int id);
        Task<IDataResult<List<Blog>>> GetLastBlogAsync(int count, int skip = 0, bool sortInReserver = true);
        Task<IResultObject> BlogAddAsync(Blog blog, string userName, IFormFile blogImage = null, IFormFile blogThumbnailImage = null);
        Task<IResultObject> BlogUpdateAsync(Blog blog, string userName, IFormFile blogImage = null, IFormFile blogThumbnailImage = null);
        Task<IResultObject> DeleteBlogAsync(Blog blog, string userName);
        Task<IResultObject> DeleteBlogByAdminAsync(Blog blog);
        Task<IDataResult<int>> GetCountAsync(Expression<Func<Blog, bool>> filter = null);
        Task<IDataResult<int>> GetBlogCountByWriterAsync(string userName);
        Task<IDataResult<List<Blog>>> GetListAsync(Expression<Func<Blog, bool>> filter = null, int take = 0);
        Task<IDataResult<List<Blog>>> GetListByReadAllLastBlogsByWriterAsync(int blogId, int writerID, int take = 0);
        Task<IDataResult<List<Blog>>> GetListByReadAllLastBlogsAsync(int blogId, int writerID, int take = 0);
        Task<IResultObject> ChangedBlogStatusAsync(int id, string userName);
        Task<IResultObject> ChangedBlogStatusByAdminAsync(int id);
        Task<IResultObject> BlogAdminUpdateAsync(Blog blog, IFormFile blogImage = null, IFormFile blogThumbnailImage = null);
        Task<IDataResult<Blog>> GetFileNameContentBlogByIDAsync(int id);
        Task<IDataResult<List<BlogCategoryandCommentCountDto>>> GetBlogListByMainPage(int id, int page = 1, int take = 6, string search = null);
        Task<IDataResult<Blog>> GetBlogByIdWithCommentAsync(int id);
    }
}
