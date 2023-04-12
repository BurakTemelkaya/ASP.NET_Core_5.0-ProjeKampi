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
using CoreDemo.Models;
using CoreLayer.Utilities.Results;
using CoreLayer.Utilities.Business;
using AutoMapper;
using CoreLayer.Aspects.AutoFac.Performance;
using EntityLayer.DTO;
using BusinessLayer.Constants;
using CoreLayer.Aspects.AutoFac.Logging;
using CoreLayer.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using CoreLayer.Aspects.AutoFac.Exception;

namespace BusinessLayer.Concrete
{
    public class BlogManager : ManagerBase, IBlogService
    {
        private readonly IBlogDal _blogDal;
        private readonly IBusinessUserService _userService;
        private readonly ICategoryService _categoryService;

        public BlogManager(IBlogDal blogDal, IBusinessUserService userService, IMapper mapper, ICategoryService categoryService) : base(mapper)
        {
            _blogDal = blogDal;
            _userService = userService;
            _categoryService = categoryService;
        }

        public async Task<IDataResult<List<Blog>>> GetBlogListWithCategoryAsync(int take = 0, Expression<Func<Blog, bool>> filter = null)
        {
            var values = await _blogDal.GetListWithCategoryandCommentAsync(filter, take);

            foreach (var item in values)
                item.BlogContent = await TextFileManager.ReadTextFileAsync(item.BlogContent, 50);

            return new SuccessDataResult<List<Blog>>(values);
        }

        public async Task<IDataResult<List<Blog>>> GetBlogListWithCategoryByPagingAsync(int take, int page, Expression<Func<Blog, bool>> filter = null)
        {
            var values = await _blogDal.GetListWithCategoryandCommentByPagingAsync(filter, take, page);

            foreach (var item in values)
            {
                if (item != null)
                {
                    item.BlogContent = await TextFileManager.ReadTextFileAsync(item.BlogContent, 50);
                }
            }

            return new SuccessDataResult<List<Blog>>(values);
        }

        public async Task<IDataResult<List<Blog>>> GetListWithCategoryByWriterBmAsync(string userName, int take, int page, Expression<Func<Blog, bool>> filter = null)
        {
            var user = await _userService.FindByUserNameAsync(userName);
            var values = await _blogDal.GetListWithCategoryByWriterandPagingAsync(user.Data.Id, filter, take, page);
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
                return new ErrorDataResult<Blog>();
            value.BlogContent = await TextFileManager.ReadTextFileAsync(value.BlogContent);
            return new SuccessDataResult<Blog>(value);
        }

        public async Task<IDataResult<Blog>> GetBlogByIdForUpdate(int id)
        {

            var value = await _blogDal.GetByIDAsync(id);

            if (value == null)
                return new ErrorDataResult<Blog>("Blog bulunamadı.");

            value.BlogThumbnailImage = null;
            value.BlogImage = null;

            value.BlogContent = await TextFileManager.ReadTextFileAsync(value.BlogContent);
            return new SuccessDataResult<Blog>(value);
        }


        public async Task<IDataResult<List<Blog>>> GetLastBlogAsync(int count)
        {
            var value = await _blogDal.GetListAllAsync(x => x.BlogStatus, count);

            if (value == null)
            {
                return new ErrorDataResult<List<Blog>>();
            }

            return new SuccessDataResult<List<Blog>>(value.OrderByDescending(x => x.BlogID).ToList());
        }

        public async Task<IDataResult<List<Blog>>> GetBlogListByWriterAsync(int id)
        {
            if (id == 0)
            {
                return new ErrorDataResult<List<Blog>>();
            }
            return new SuccessDataResult<List<Blog>>(await _blogDal.GetListAllAsync(x => x.WriterID == id));
        }

        [ValidationAspect(typeof(BlogValidator))]
        public async Task<IResult> BlogAddAsync(Blog blog, string userName, IFormFile blogImage, IFormFile blogThumbnailImage)
        {
            var user = await _userService.FindByUserNameAsync(userName);

            if (blog.BlogImage != null)
            {
                var image = ImageFileManager.DownloadImage(blog.BlogImage);
                if (image == null)
                {
                    return new ErrorResult("Blog resmi, girdiğiniz linkten getirilemedi.");
                }
                blog.BlogImage = ImageFileManager.ImageAdd(image, ImageLocations.StaticBlogImageLocation(), ImageResulotions.GetBlogImageResolution());
            }
            else if (blogImage != null)
            {
                blog.BlogImage = ImageFileManager.ImageAdd(blogImage, ImageLocations.StaticBlogImageLocation(), ImageResulotions.GetBlogImageResolution());
            }

            if (blog.BlogImage == null)
            {
                return new ErrorResult("Blog resmi yüklenemedi.");
            }

            if (blog.BlogThumbnailImage != null)
            {
                var image = ImageFileManager.DownloadImage(blog.BlogThumbnailImage);
                if (image == null)
                {
                    return new ErrorResult("Blog küçük resmi, girdiğiniz linkten getirilemedi.");
                }
                blog.BlogThumbnailImage = ImageFileManager.ImageAdd(image, ImageLocations.StaticBlogImageLocation(), ImageResulotions.GetBlogThumbnailResolution());
            }
            else if (blogThumbnailImage != null)
            {
                blog.BlogThumbnailImage = ImageFileManager.ImageAdd(blogThumbnailImage, ImageLocations.StaticBlogImageLocation(), ImageResulotions.GetBlogThumbnailResolution());
            }

            if (blog.BlogThumbnailImage == null)
            {
                return new ErrorResult("Blog küçük resmi yüklenemedi.");
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

        [ValidationAspect(typeof(BlogValidator))]
        public async Task<IResult> BlogUpdateAsync(Blog blog, string userName, IFormFile blogImage = null, IFormFile blogThumbnailImage = null)
        {
            var user = await _userService.FindByUserNameAsync(userName);
            var oldValueRaw = await GetBlogByIDAsync(blog.BlogID);
            var oldValue = oldValueRaw.Data;

            blog.WriterID = oldValue.WriterID;
            blog.BlogCreateDate = oldValue.BlogCreateDate;

            if (user == null || user.Data.Id != oldValue.WriterID)
                return new ErrorDataResult<Blog>(blog);

            if (blog.BlogImage == null && blogImage == null)
            {
                blog.BlogImage = oldValue.BlogImage;
            }
            else if (blog.BlogImage != null)
            {
                var image = ImageFileManager.DownloadImage(blog.BlogImage);
                if (image == null)
                {
                    return new ErrorResult("Blog resmi, girdiğiniz linkten getirilemedi.");
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
                return new ErrorResult("Blog resmi, yüklenemedi.");
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
                    return new ErrorResult("Blog küçük resmi, girdiğiniz linkten getirilemedi.");
                }
                blog.BlogThumbnailImage = ImageFileManager.ImageAdd(image, ImageLocations.StaticBlogImageLocation(), ImageResulotions.GetBlogThumbnailResolution());
                DeleteFileManager.DeleteFile(oldValue.BlogContent);
            }
            else if (blogThumbnailImage != null)
            {
                blog.BlogThumbnailImage = ImageFileManager.ImageAdd(blogThumbnailImage, ImageLocations.StaticBlogImageLocation(), ImageResulotions.GetBlogThumbnailResolution());
                if (blog.BlogThumbnailImage == null)
                {
                    return new ErrorResult("Blog küçük resmi, yüklenen resim ile güncellenemedi.");
                }
                DeleteFileManager.DeleteFile(oldValue.BlogContent);
            }

            if (blog.BlogThumbnailImage == null)
            {
                return new ErrorResult("Blog küçük resmi, yüklenemedi.");
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

        [ValidationAspect(typeof(BlogValidator))]
        public async Task<IResult> BlogAdminUpdateAsync(Blog blog, IFormFile blogImage = null, IFormFile blogThumbnailImage = null)
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
                    return new ErrorResult("Blog resmi, girdiğiniz linkten getirilemedi.");
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
                return new ErrorResult("Blog resmi, yüklenemedi");
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
                    return new ErrorResult("Blog küçük resmi, girdiğiniz linkten getirilemedi.");
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
                return new ErrorResult("Blog resmi, yüklenemedi");
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

        public async Task<IResult> DeleteBlogAsync(Blog blog, string userName)
        {
            var user = await _userService.FindByUserNameAsync(userName);
            if (user.Data.Id == blog.WriterID)
            {
                await _blogDal.DeleteAsync(blog);
                DeleteFileManager.DeleteFile(blog.BlogContent);
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        public async Task<IDataResult<int>> GetCountAsync(Expression<Func<Blog, bool>> filter = null)
        {
            return new SuccessDataResult<int>(await _blogDal.GetCountAsync(filter));
        }

        public async Task<IDataResult<List<Blog>>> GetListAsync(Expression<Func<Blog, bool>> filter = null, int take = 0)
        {
            var values = await _blogDal.GetListAllAsync(filter, take);
            foreach (var item in values)
                item.BlogContent = await TextFileManager.ReadTextFileAsync(item.BlogContent, 50);
            return new SuccessDataResult<List<Blog>>(values.OrderByDescending(x => x.BlogID).ToList());
        }

        public async Task<IResult> ChangedBlogStatusAsync(int id, string userName)
        {
            var blog = await GetFileNameContentBlogByIDAsync(id);
            var user = await _userService.FindByUserNameAsync(userName);
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
            return new ErrorResult("Kullanıcı bulunamadı.");
        }

        public async Task<IDataResult<Blog>> GetFileNameContentBlogByIDAsync(int id)
        {
            return new SuccessDataResult<Blog>(await _blogDal.GetByIDAsync(id));
        }

        public async Task<IResult> DeleteBlogByAdminAsync(Blog blog)
        {
            await _blogDal.DeleteAsync(blog);
            DeleteFileManager.DeleteFile(blog.BlogContent);
            return new SuccessResult();
        }

        public async Task<IResult> ChangedBlogStatusByAdminAsync(int id)
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

        public async Task<IDataResult<List<Blog>>> GetBlogListByMainPage(string id, int page = 1, int take = 6, string search = null)
        {
            List<Blog> values = new();

            bool isSuccess = true;

            var message = string.Empty;

            if (id == null && search == null)
            {
                values = await _blogDal.GetListWithCategoryandCommentByPagingAsync(x => x.BlogStatus && x.Category.CategoryStatus, take, page);
            }

            if (id != null && search == null)
            {
                var category = await _categoryService.TGetByIDAsync(Convert.ToInt32(id));
                var categoryCount = await GetCountAsync(x => x.CategoryID == Convert.ToInt32(id));
                if (categoryCount.Data != 0)
                {
                    values = await _blogDal.GetListWithCategoryandCommentByPagingAsync(x => x.Category.CategoryStatus &&
                    x.CategoryID == Convert.ToInt32(id), take, page);
                    message = category.Data.CategoryName + " kategorisindeki bloglar.";
                    if (values == null)
                    {
                        values = await _blogDal.GetListWithCategoryandCommentByPagingAsync(x => x.BlogStatus && x.Category.CategoryStatus, take, page);
                        isSuccess = false;
                        message = "Şu anda " + category.Data.CategoryName + " kategorisinde blog bulunmamaktadır.";
                    }
                }
            }

            if (search != null)
            {
                if (id == null)
                {
                    values = await _blogDal.GetListWithCategoryandCommentByPagingAsync(x => x.BlogTitle.ToLower().Contains(search.ToLower()), take, page);
                    message = "'" + search + "' aramanıza dair sonuçlar.";
                }
                else
                {
                    values = await _blogDal.GetListWithCategoryandCommentByPagingAsync(x => x.BlogTitle.ToLower().Contains(search.ToLower()) && x.CategoryID == Convert.ToInt32(id), take, page);
                    message = values.First().Category.CategoryName + " kategorisindeki " + search + " aramanıza dair sonuçlar.";
                }
                if (values.Count == 0)
                {
                    isSuccess = false;
                    values = await _blogDal.GetListWithCategoryandCommentByPagingAsync(x => x.BlogStatus && x.Category.CategoryStatus, take, page);
                    message = "'" + search + "' aramanıza dair sonuç bulunamadı.";
                }
            }

            foreach (var item in values)
            {
                if (item != null)
                {
                    item.BlogContent = await TextFileManager.ReadTextFileAsync(item.BlogContent, 50);
                }

            }

            return new DataResult<List<Blog>>(values, isSuccess, message);
        }

        IResult BlogImageNotEmpty(string blogImage)
        {
            if (blogImage == string.Empty)
            {
                return new ErrorResult("Lütfen blog resminizin linkini giriniz veya yükleyin.");
            }
            return new SuccessResult();
        }

        IResult BlogThumbnailNotEmpty(string blogThumnailImage)
        {
            if (blogThumnailImage == string.Empty)
            {
                return new ErrorResult("Lütfen blog küçük resminizin linkini giriniz veya yükleyin.");
            }
            return new SuccessResult();
        }
    }
}
