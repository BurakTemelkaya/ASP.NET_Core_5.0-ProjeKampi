using BusinessLayer.Abstract;
using BusinessLayer.Constants;
using BusinessLayer.ValidationRules;
using CoreLayer.Aspects.AutoFac.Caching;
using CoreLayer.Aspects.AutoFac.Validation;
using CoreLayer.Utilities.FileUtilities;
using CoreLayer.Utilities.Results;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class AboutManager : IAboutService
    {
        private readonly IAboutDal _aboutDal;

        public AboutManager(IAboutDal aboutDal)
        {
            _aboutDal = aboutDal;
        }

        [CacheAspect]
        public async Task<IDataResult<About>> GetAboutAsync()
        {
            return new SuccessDataResult<About>(await _aboutDal.GetByFilterAsync());
        }

        [CacheAspect]
        public async Task<IDataResult<About>> GetAboutByFooterAsync()
        {
            var value = await _aboutDal.GetByFilterAsync();

            if (value.AboutDetails1.Length > 475)
            {
                value.AboutDetails1 = value.AboutDetails1[..475] + "...";
            }

            return new SuccessDataResult<About>(value);
        }

        public async Task<IDataResult<About>> GetAboutByUpdateAsync()
        {
            var result = await _aboutDal.GetByFilterAsync();
            result.AboutImage1 = null;
            result.AboutImage2 = null;
            return new SuccessDataResult<About>(result);
        }

        [ValidationAspect(typeof(AboutValidator))]
        [CacheRemoveAspect("IAboutService.Get")]
        public async Task<IResult> UpdateAsync(About about, IFormFile aboutImage1, IFormFile aboutImage2)
        {
            var oldValue = await GetAboutAsync();

            about.AboutID = oldValue.Data.AboutID;

            about.AboutStatus = true;

            if (about.AboutImage1 == null && aboutImage1 == null)
            {
                about.AboutImage1 = oldValue.Data.AboutImage1;
            }
            else if (aboutImage1 != null)
            {
                about.AboutImage1 = ImageFileManager.ImageAdd(aboutImage1, ImageLocations.StaticAboutImageLocation(), ImageResulotions.GetAboutImageResolution());
                if (about.AboutImage1 == null)
                {
                    return new ErrorResult("1. Hakkında resmi yüklenemedi");
                }
                DeleteFileManager.DeleteFile(oldValue.Data.AboutImage1);
            }
            else if (about.AboutImage1 != null)
            {
                about.AboutImage1 = ImageFileManager.ImageAdd(ImageFileManager.DownloadImage(about.AboutImage1), ImageLocations.StaticAboutImageLocation(), ImageResulotions.GetAboutImageResolution());
                if (about.AboutImage1 == null)
                {
                    return new ErrorResult("1. Hakkında resmi yüklenemedi");
                }
                DeleteFileManager.DeleteFile(oldValue.Data.AboutImage1);
            }


            if (about.AboutImage2 == null && aboutImage2 == null)
            {
                about.AboutImage2 = oldValue.Data.AboutImage2;
            }
            else if (aboutImage2 != null)
            {
                about.AboutImage2 = ImageFileManager.ImageAdd(aboutImage2, ImageLocations.StaticAboutImageLocation(), ImageResulotions.GetAboutImageResolution());
                if (aboutImage2 == null)
                {
                    return new ErrorResult("2. Hakkında resmi yüklenemedi");
                }
                DeleteFileManager.DeleteFile(oldValue.Data.AboutImage2);
            }
            else if (about.AboutImage2 != null)
            {
                about.AboutImage2 = ImageFileManager.ImageAdd(ImageFileManager.DownloadImage(about.AboutImage2), ImageLocations.StaticAboutImageLocation(), ImageResulotions.GetAboutImageResolution());
                if (aboutImage1 == null)
                {
                    return new ErrorResult("2. Hakkında resmi yüklenemedi");
                }
                DeleteFileManager.DeleteFile(oldValue.Data.AboutImage2);
            }
            await _aboutDal.UpdateAsync(about);
            return new SuccessResult();
        }
    }
}
