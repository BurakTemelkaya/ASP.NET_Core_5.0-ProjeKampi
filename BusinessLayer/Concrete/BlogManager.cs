﻿using BusinessLayer.Abstract;
using BusinessLayer.ValidationRules;
using CoreLayer.Aspects.AutoFac.Validation;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreLayer.Utilities.FileUtilities;
using CoreLayer.Utilities.Results;
using CoreLayer.Utilities.Business;
using AutoMapper;
using EntityLayer.DTO;
using BusinessLayer.Constants;
using CoreLayer.Aspects.AutoFac.Caching;
using BusinessLayer.StaticTexts;

namespace BusinessLayer.Concrete
{
    public class BlogManager : ManagerBase, IBlogService
    {
        private readonly IBlogDal _blogDal;
        private readonly IBusinessUserService _userService;
        private readonly ICategoryService _categoryService;
        private readonly IHttpContextAccessor _contextAccessor;

        public BlogManager(IBlogDal blogDal, IBusinessUserService userService, IMapper mapper, ICategoryService categoryService
            , IHttpContextAccessor contextAccessor) : base(mapper)
        {
            _blogDal = blogDal;
            _userService = userService;
            _categoryService = categoryService;
            _contextAccessor = contextAccessor;
        }

        [CacheAspect]
        public async Task<IDataResult<List<BlogCategoryandCommentCountDto>>> GetBlogListWithCategoryandCommentCountAsync(int take = 0, Expression<Func<BlogCategoryandCommentCountDto, bool>> filter = null)
        {
            var values = await _blogDal.GetBlogListWithCategoryandCommentCountAsync(filter, true, take);

            foreach (var item in values)
                item.BlogContent = await TextFileManager.ReadTextFileAsync(item.BlogContent, 50);

            return new SuccessDataResult<List<BlogCategoryandCommentCountDto>>(values);
        }


        public async Task<IDataResult<List<BlogCategoryandCommentCountDto>>> GetBlogListWithCategoryandCommentCountByPagingAsync(int take, int page, Expression<Func<BlogCategoryandCommentCountDto, bool>> filter = null)
        {
            var values = await _blogDal.GetListWithCategoryandCommentCountByPagingAsync(filter, true, take, page);

            foreach (var item in values)
            {
                if (item != null)
                {
                    item.BlogContent = await TextFileManager.ReadTextFileAsync(item.BlogContent, 50);
                }
            }

            return new SuccessDataResult<List<BlogCategoryandCommentCountDto>>(values);
        }

        [CacheAspect]
        public async Task<IDataResult<List<Blog>>> GetListWithCategoryByWriterWitchPagingAsync(string userName, int take, int page)
        {
            var user = await _userService.GetByUserNameAsync(userName);
            var values = await _blogDal.GetListWithCategoryByPagingAsync(
                x => x.WriterID == user.Data.Id, take, page);
            foreach (var item in values)
            {
                if (item != null)
                {
                    item.BlogContent = await TextFileManager.ReadTextFileAsync(item.BlogContent, 50);
                }
            }
            return new SuccessDataResult<List<Blog>>(values);
        }

        [CacheAspect]
        public async Task<IDataResult<List<Blog>>> GetListWithCategory(Expression<Func<Blog, bool>> filter = null, int take = 0, int skip = 0)
        {
            var values = await _blogDal.GetListBlogWithCategoryAsync(filter, take);
            return new SuccessDataResult<List<Blog>>(values);
        }

        public async Task<IDataResult<List<Blog>>> GetListWithCategoryByPaging(int take = 0, int page = 1, Expression<Func<Blog, bool>> filter = null)
        {
            var values = await _blogDal.GetListWithCategoryByPagingAsync(filter, take, page);
            foreach (var item in values)
            {
                if (item != null)
                {
                    item.BlogContent = await TextFileManager.ReadTextFileAsync(item.BlogContent, 50);
                }
            }
            return new SuccessDataResult<List<Blog>>(values);
        }

        public async Task<IDataResult<Blog>> GetBlogByIDAsync(int id)
        {
            var value = await _blogDal.GetByIDAsync(id);
            if (value == null)
                return new ErrorDataResult<Blog>(Messages.BlogNotFound);
            value.BlogContent = await TextFileManager.ReadTextFileAsync(value.BlogContent);
            return new SuccessDataResult<Blog>(value);
        }

        [CacheRemoveAspect("IBlogService.Get")]
        public async Task<IDataResult<Blog>> GetBlogByIdForUpdate(int id)
        {
            var value = await _blogDal.GetByIDAsync(id);

            if (value == null)
                return new ErrorDataResult<Blog>(Messages.BlogNotFound);

            var user = await _userService.GetByUserNameAsync(_contextAccessor.HttpContext.User.Identity.Name);

            var isAdmin = _contextAccessor.HttpContext.User.IsInRole("Admin");

            if (!isAdmin && user.Data.Id != value.WriterID)
                return new ErrorDataResult<Blog>(Messages.BlogDoesNotBelongToYouCannotBeUpdated);

            value.BlogThumbnailImage = null;
            value.BlogImage = null;

            value.BlogContent = await TextFileManager.ReadTextFileAsync(value.BlogContent);
            return new SuccessDataResult<Blog>(value);
        }

        [CacheAspect]
        public async Task<IDataResult<List<Blog>>> GetLastBlogAsync(int count, int skip = 0, bool sortInReverse = true)
        {
            var value = await _blogDal.GetListAllAsync(x => x.BlogStatus, count, skip, sortInReverse);

            if (value == null)
            {
                return new ErrorDataResult<List<Blog>>(Messages.BlogNotFound);
            }

            return new SuccessDataResult<List<Blog>>(value);
        }

        [CacheAspect]
        public async Task<IDataResult<List<Blog>>> GetBlogListByWriterAsync(int id)
        {
            if (id == 0)
            {
                return new ErrorDataResult<List<Blog>>(Messages.BlogNotFound);
            }
            return new SuccessDataResult<List<Blog>>(await _blogDal.GetListAllAsync(x => x.WriterID == id));
        }

        [CacheRemoveAspect("IBlogService.Get")]
        [CacheRemoveAspect("ICategoryService.Get")]
        [ValidationAspect(typeof(BlogValidator))]
        public async Task<IResultObject> BlogAddAsync(Blog blog, string userName, IFormFile blogImage, IFormFile blogThumbnailImage)
        {
            var user = await _userService.GetByUserNameAsync(userName);

            var categoryStatusResult = await CheckCategoryStatusAsync(user, blog);

            if (!categoryStatusResult.Success)
            {
                return categoryStatusResult;
            }

            if (blog.BlogImage != null)
            {
                var image = ImageFileManager.DownloadImage(blog.BlogImage);
                if (image == null)
                {
                    return new ErrorResult(Messages.BlogImageNotGetting);
                }
                blog.BlogImage = ImageFileManager.ImageAdd(image, ImageLocations.StaticBlogImageLocation(), ImageResulotions.GetBlogImageResolution());
            }
            else if (blogImage != null)
            {
                blog.BlogImage = ImageFileManager.ImageAdd(blogImage, ImageLocations.StaticBlogImageLocation(), ImageResulotions.GetBlogImageResolution());
            }

            if (blog.BlogImage == null)
            {
                return new ErrorResult(Messages.BlogImageNotUploading);
            }

            if (blog.BlogThumbnailImage != null)
            {
                var image = ImageFileManager.DownloadImage(blog.BlogThumbnailImage);
                if (image == null)
                {
                    return new ErrorResult(Messages.BlogThumbnailNotGetting);
                }
                blog.BlogThumbnailImage = ImageFileManager.ImageAdd(image, ImageLocations.StaticBlogImageLocation(), ImageResulotions.GetBlogThumbnailResolution());
            }
            else if (blogThumbnailImage != null)
            {
                blog.BlogThumbnailImage = ImageFileManager.ImageAdd(blogThumbnailImage, ImageLocations.StaticBlogImageLocation(), ImageResulotions.GetBlogThumbnailResolution());
            }

            if (blog.BlogThumbnailImage == null)
            {
                return new ErrorResult(Messages.BlogThumbnailNotUploading);
            }

            var result = BusinessRules.Run(UserNotEmpty(user), BlogImageNotEmpty(blog.BlogImage), BlogThumbnailNotEmpty(blog.BlogThumbnailImage));

            if (!result.Success)
            {
                return result;
            }

            blog.BlogContent = await TextFileManager.TextFileAddAsync(blog.BlogContent, ContentFileLocations.GetBlogContentFileLocation());
            blog.WriterID = user.Data.Id;
            blog.BlogCreateDate = DateTime.Now;
            await _blogDal.InsertAsync(blog);
            return new SuccessResult();
        }

        [CacheRemoveAspect("IBlogService.Get")]
        [CacheRemoveAspect("ICategoryService.Get")]
        [ValidationAspect(typeof(BlogValidator))]
        public async Task<IResultObject> BlogUpdateAsync(Blog blog, string userName, IFormFile blogImage = null, IFormFile blogThumbnailImage = null)
        {
            var user = await _userService.GetByUserNameAsync(userName);
            var oldValueRaw = await GetBlogByIDAsync(blog.BlogID);
            var oldValue = oldValueRaw.Data;

            var categoryStatusResult = await CheckCategoryStatusAsync(user, blog);

            if (!categoryStatusResult.Success)
            {
                return categoryStatusResult;
            }

            blog.WriterID = oldValue.WriterID;
            blog.BlogCreateDate = oldValue.BlogCreateDate;

            if (user == null || user.Data.Id != oldValue.WriterID)
                return new ErrorDataResult<Blog>(blog, Messages.BlogDoesNotBelongToYouCannotBeUpdated);

            if (blog.BlogImage == null && blogImage == null)
            {
                blog.BlogImage = oldValue.BlogImage;
            }
            else if (blog.BlogImage != null)
            {
                var image = ImageFileManager.DownloadImage(blog.BlogImage);
                if (image == null)
                {
                    return new ErrorResult(Messages.BlogImageNotGetting);
                }
                blog.BlogImage = ImageFileManager.ImageAdd(image, ImageLocations.StaticBlogImageLocation(), ImageResulotions.GetBlogImageResolution());
            }
            else if (blogImage != null)
            {
                DeleteFileManager.DeleteFile(oldValue.BlogImage);
                blog.BlogImage = ImageFileManager.ImageAdd(blogImage, ImageLocations.StaticBlogImageLocation(), ImageResulotions.GetBlogImageResolution());
            }

            if (blog.BlogImage == null)
            {
                return new ErrorResult(Messages.BlogImageNotUploading);
            }


            if (blog.BlogThumbnailImage == null && blogThumbnailImage == null)
            {
                blog.BlogThumbnailImage = oldValue.BlogThumbnailImage;
            }
            else if (blog.BlogThumbnailImage != null)
            {
                var image = ImageFileManager.DownloadImage(blog.BlogThumbnailImage);
                if (image == null)
                {
                    return new ErrorResult(Messages.BlogThumbnailNotGetting);
                }
                blog.BlogThumbnailImage = ImageFileManager.ImageAdd(image, ImageLocations.StaticBlogImageLocation(), ImageResulotions.GetBlogThumbnailResolution());
                DeleteFileManager.DeleteFile(oldValue.BlogContent);
            }
            else if (blogThumbnailImage != null)
            {
                blog.BlogThumbnailImage = ImageFileManager.ImageAdd(blogThumbnailImage, ImageLocations.StaticBlogImageLocation(), ImageResulotions.GetBlogThumbnailResolution());
                if (blog.BlogThumbnailImage == null)
                {
                    return new ErrorResult(Messages.BlogThumbnailNotGetting);
                }
                DeleteFileManager.DeleteFile(oldValue.BlogContent);
            }

            if (blog.BlogThumbnailImage == null)
            {
                return new ErrorResult(Messages.BlogThumbnailNotUploading);
            }


            var oldBlogValue = await GetFileNameContentBlogByIDAsync(blog.BlogID);
            if (blog.BlogContent != oldValue.BlogContent)
            {
                blog.BlogContent = await TextFileManager.TextFileAddAsync(blog.BlogContent, ContentFileLocations.GetBlogContentFileLocation());
            }
            else
                blog.BlogContent = oldBlogValue.Data.BlogContent;
            await _blogDal.UpdateAsync(blog);
            return new SuccessResult();
        }

        [CacheRemoveAspect("IBlogService.Get")]
        [CacheRemoveAspect("ICategoryService.Get")]
        [ValidationAspect(typeof(BlogValidator))]
        public async Task<IResultObject> BlogAdminUpdateAsync(Blog blog, IFormFile blogImage = null, IFormFile blogThumbnailImage = null)
        {
            var OldValueRaw = await GetBlogByIDAsync(blog.BlogID);
            var oldValue = OldValueRaw.Data;

            blog.WriterID = oldValue.WriterID;
            blog.BlogCreateDate = oldValue.BlogCreateDate;

            if (blog.BlogImage == null && blogImage == null)
            {
                blog.BlogImage = oldValue.BlogImage;
            }
            else if (blog.BlogImage != null)
            {
                var image = ImageFileManager.DownloadImage(blog.BlogImage);
                if (image == null)
                {
                    return new ErrorResult(Messages.BlogImageNotGetting);
                }
                blog.BlogImage = ImageFileManager.ImageAdd(image, ImageLocations.StaticBlogImageLocation(), ImageResulotions.GetBlogImageResolution());
            }
            else if (blogImage != null)
            {
                DeleteFileManager.DeleteFile(oldValue.BlogImage);
                blog.BlogImage = ImageFileManager.ImageAdd(blogImage, ImageLocations.StaticBlogImageLocation(), ImageResulotions.GetBlogImageResolution());
            }

            if (blog.BlogImage == null)
            {
                return new ErrorResult(Messages.BlogImageNotUploading);
            }

            if (blog.BlogThumbnailImage == null && blogThumbnailImage == null)
            {
                blog.BlogThumbnailImage = oldValue.BlogThumbnailImage;
            }
            else if (blog.BlogThumbnailImage != null)
            {
                var image = ImageFileManager.DownloadImage(blog.BlogThumbnailImage);
                if (image == null)
                {
                    return new ErrorResult(Messages.BlogThumbnailNotGetting);
                }
                blog.BlogThumbnailImage = ImageFileManager.ImageAdd(image, ImageLocations.StaticBlogImageLocation(), ImageResulotions.GetBlogThumbnailResolution());
                DeleteFileManager.DeleteFile(oldValue.BlogThumbnailImage);
            }
            else if (blogThumbnailImage != null)
            {
                blog.BlogThumbnailImage = ImageFileManager.ImageAdd(blogThumbnailImage, ImageLocations.StaticBlogImageLocation(), ImageResulotions.GetBlogThumbnailResolution());
                DeleteFileManager.DeleteFile(oldValue.BlogThumbnailImage);
            }

            if (blog.BlogThumbnailImage == null)
            {
                return new ErrorResult(Messages.BlogThumbnailNotUploading);
            }


            var oldBlogValue = await GetFileNameContentBlogByIDAsync(blog.BlogID);
            if (blog.BlogContent != oldValue.BlogContent)
            {
                blog.BlogContent = await TextFileManager.TextFileAddAsync(blog.BlogContent, ContentFileLocations.GetBlogContentFileLocation());
            }
            else
                blog.BlogContent = oldBlogValue.Data.BlogContent;
            await _blogDal.UpdateAsync(blog);

            return new SuccessResult();
        }

        [CacheRemoveAspect("ICategoryService.Get")]
        [CacheRemoveAspect("IBlogService.Get")]
        public async Task<IResultObject> DeleteBlogAsync(Blog blog, string userName)
        {
            var user = await _userService.GetByUserNameAsync(userName);
            if (user.Data.Id == blog.WriterID)
            {
                await _blogDal.DeleteAsync(blog);
                DeleteFileManager.DeleteFile(blog.BlogContent);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        [CacheAspect]
        public async Task<IDataResult<int>> GetCountAsync(Expression<Func<Blog, bool>> filter = null)
        {
            return new SuccessDataResult<int>(await _blogDal.GetCountAsync(filter));
        }

        [CacheAspect]
        public async Task<IDataResult<int>> GetBlogCountByWriterAsync(string userName)
        {
            var user = await _userService.GetByUserNameAsync(userName);
            if (!user.Success)
            {
                return new ErrorDataResult<int>(Messages.UserNotFound);
            }
            return new SuccessDataResult<int>(await _blogDal.GetCountAsync(x => x.WriterID == user.Data.Id));
        }

        [CacheAspect]
        public async Task<IDataResult<List<Blog>>> GetListAsync(Expression<Func<Blog, bool>> filter = null, int take = 0)
        {
            var values = await _blogDal.GetListAllAsync(filter, take);
            foreach (var item in values)
                item.BlogContent = await TextFileManager.ReadTextFileAsync(item.BlogContent, 50);
            return new SuccessDataResult<List<Blog>>(values.OrderByDescending(x => x.BlogID).ToList());
        }

        [CacheAspect]
        public async Task<IDataResult<List<Blog>>> GetListByReadAllLastBlogsByWriterAsync(int blogId, int writerID, int take = 0)
        {
            var values = await _blogDal.GetListAllAsync(x => x.BlogID != blogId && x.WriterID == writerID, take);
            foreach (var item in values)
                item.BlogContent = await TextFileManager.ReadTextFileAsync(item.BlogContent, 50);
            return new SuccessDataResult<List<Blog>>(values.OrderByDescending(x => x.BlogID).ToList());
        }

        [CacheAspect]
        public async Task<IDataResult<List<Blog>>> GetListByReadAllLastBlogsAsync(int blogId, int writerID, int take = 0)
        {
            var values = await _blogDal.GetListAllAsync(x => x.BlogID != blogId && x.WriterID != writerID, take);
            foreach (var item in values)
                item.BlogContent = await TextFileManager.ReadTextFileAsync(item.BlogContent, 50);
            return new SuccessDataResult<List<Blog>>(values.OrderByDescending(x => x.BlogID).ToList());
        }

        [CacheRemoveAspect("ICategoryService.Get")]
        [CacheRemoveAspect("IBlogService.Get")]
        public async Task<IResultObject> ChangedBlogStatusAsync(int id, string userName)
        {
            var blog = await GetFileNameContentBlogByIDAsync(id);
            var user = await _userService.GetByUserNameAsync(userName);
            if (user.Data.Id == blog.Data.WriterID)
            {
                var value = await _blogDal.GetByIDAsync(blog.Data.BlogID);
                if (value.BlogStatus)
                    value.BlogStatus = false;
                else
                    value.BlogStatus = true;

                await _blogDal.UpdateAsync(value);
                return new SuccessResult();
            }
            return new ErrorResult(Messages.UserNotFound);
        }

        public async Task<IDataResult<Blog>> GetFileNameContentBlogByIDAsync(int id)
        {
            return new SuccessDataResult<Blog>(await _blogDal.GetByIDAsync(id));
        }

        [CacheRemoveAspect("ICategoryService.Get")]
        [CacheRemoveAspect("IBlogService.Get")]
        public async Task<IResultObject> DeleteBlogByAdminAsync(Blog blog)
        {
            await _blogDal.DeleteAsync(blog);
            DeleteFileManager.DeleteFile(blog.BlogContent);
            return new SuccessResult();
        }

        [CacheRemoveAspect("ICategoryService.Get")]
        [CacheRemoveAspect("IBlogService.Get")]
        public async Task<IResultObject> ChangedBlogStatusByAdminAsync(int id)
        {
            var blog = await GetFileNameContentBlogByIDAsync(id);
            var value = await _blogDal.GetByIDAsync(blog.Data.BlogID);
            if (value.BlogStatus)
                value.BlogStatus = false;
            else
                value.BlogStatus = true;
            await _blogDal.UpdateAsync(value);

            return new SuccessResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Kategori id değerine göre filtreleme yapmak için gerekli değer.</param>
        /// <param name="page">O anki sayfa sayısını almak ve sayfalama sistemi kullanabilmek için gerekli parametre.</param>
        /// <param name="take">Page paramatresi 0'dan büyük ise sayfalama değil ise verilen sayı kadar veri gelmesini</param>
        /// <param name="search">Bloglar içinde başlığa göre filtreleme yapmak için gerekli parametre.</param>
        /// <returns></returns>
        [CacheAspect]
        public async Task<IDataResult<List<BlogCategoryandCommentCountDto>>> GetBlogListByMainPage(int id = 0, int page = 1, int take = 6, string search = null)
        {
            List<BlogCategoryandCommentCountDto> values = null;

            bool isSuccess = true;

            string message = string.Empty;

            values = await _blogDal.GetListWithCategoryandCommentCountByPagingAsync(
                        x => (x.BlogStatus == true && x.CategoryStatus == true)
                        && (id < 1 || x.CategoryID == id)
                        && (string.IsNullOrEmpty(search) || x.BlogTitle.ToLower().Contains(search.ToLower()))
                        , true, take, page
                    );

            if (!values.Any())
            {
                message = "Arama kriterlerinize uygun sonuç bulunamadı.";
                isSuccess = false;
                values = await _blogDal.GetListWithCategoryandCommentCountByPagingAsync(x => x.BlogStatus && x.CategoryStatus, true, take, page);
            }
            else
            {
                if (id > 0)
                {
                    var category = await _categoryService.TGetByIDAsync(Convert.ToInt32(id));

                    message = category.Success ? category.Data.CategoryName + " kategorisindeki bloglar." : "";
                }

                message += search != null ? " Aranan kelime = " + search : "";
            }

            foreach (var item in values)
            {
                if (item != null)
                {
                    item.BlogContent = await TextFileManager.ReadTextFileAsync(item.BlogContent, 50);
                }
            }

            return new DataResult<List<BlogCategoryandCommentCountDto>>(values, isSuccess, message);
        }

        public async Task<IDataResult<BlogCategoryandCommentCountandWriterDto>> GetBlogByIdWithCommentAsync(int id)
        {
            var result = await _blogDal.GetBlogWithCommentandWriterAsync(true, x => x.BlogID == id);
            if (result != null)
            {
                result.BlogContent = await TextFileManager.ReadTextFileAsync(result.BlogContent);
                return new SuccessDataResult<BlogCategoryandCommentCountandWriterDto>(result);
            }
            return new ErrorDataResult<BlogCategoryandCommentCountandWriterDto>(Messages.BlogNotFound);
        }

        IResultObject BlogImageNotEmpty(string blogImage)
        {
            if (blogImage == string.Empty)
            {
                return new ErrorResult(Messages.BlogImageNotEmpty);
            }
            return new SuccessResult();
        }

        IResultObject BlogThumbnailNotEmpty(string blogThumnailImage)
        {
            if (blogThumnailImage == string.Empty)
            {
                return new ErrorResult(Messages.BlogThumbnailNotEmpty);
            }
            return new SuccessResult();
        }

        async Task<IResultObject> CheckCategoryStatusAsync(IDataResult<UserDto> user, Blog blog)
        {
            var roles = await _userService.GetUserRoleListAsync(user.Data);

            bool isAdminorModerator = roles.Data.Count(x => x == RolesTexts.AdminRole() || x == RolesTexts.ModeratorRole()) > 0;

            if (!isAdminorModerator)
            {
                //kategori pasif ise bu kategoride adminler hariç blog eklenemez.
                var categoryList = await _categoryService.GetListAsync();
                var category = categoryList.Data.Find(x => x.CategoryID == blog.CategoryID);
                if (!category.CategoryStatus)
                {
                    return new ErrorResult(Messages.BlogCategoryIsPassiveNotAdded);
                }
            }
            return new SuccessResult();
        }

        /// <summary>
        /// Resim isimlerinin sadece guid kalacak şekilde yeniden adlandırılmasını sağlayan metod.
        /// </summary>
        /// <returns></returns>
        private async Task MoveBlogImageFileAsync()
        {
            var blogs = await _blogDal.GetListAllAsync();

            foreach (var blog in blogs)
            {
                try
                {
                    if (blog.BlogImage == null || blog.BlogImage == string.Empty)
                        continue;

                    int index = blog.BlogImage.LastIndexOf('-', blog.BlogImage.Length - 41);

                    string oldName = blog.BlogImage;

                    string newName = blog.BlogImage.Substring(index + 1);

                    bool isMove = await FileManager.FileMoveAsync(oldName, newName);

                    if (isMove)
                        blog.BlogImage = newName;

                    if (blog.BlogThumbnailImage == null || blog.BlogThumbnailImage == string.Empty)
                        continue;


                    int indexThumbnail = blog.BlogThumbnailImage.LastIndexOf('-', blog.BlogThumbnailImage.Length - 41);

                    string oldNameThumbnail = blog.BlogThumbnailImage;

                    string newNameThumbnail = blog.BlogThumbnailImage.Substring(indexThumbnail + 1);

                    bool isMoveThumbnail = await FileManager.FileMoveAsync(oldNameThumbnail, newNameThumbnail);

                    if (isMoveThumbnail)
                        blog.BlogThumbnailImage = newNameThumbnail;
                }
                catch
                {

                }
            }

            await _blogDal.UpdateRangeAsync(blogs);
        }
    }
}
