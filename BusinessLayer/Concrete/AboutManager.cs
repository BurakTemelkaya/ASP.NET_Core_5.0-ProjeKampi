﻿using BusinessLayer.Abstract;
using BusinessLayer.Constants;
using BusinessLayer.ValidationRules;
using CoreLayer.Aspects.AutoFac.Caching;
using CoreLayer.Aspects.AutoFac.Validation;
using CoreLayer.Utilities.FileUtilities;
using CoreLayer.Utilities.Results;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete;

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
    public async Task<IResultObject> UpdateAsync(About about, IFormFile aboutImage1, IFormFile aboutImage2)
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
            about.AboutImage1 = await ImageFileManager.ImageAddAsync(aboutImage1, ContentFileLocations.StaticAboutImageLocation(), ImageResulotions.GetAboutImageResolution());
            if (about.AboutImage1 == null)
            {
                return new ErrorResult(Messages.About1ImageNotUploaded);
            }
            DeleteFileManager.DeleteFile(oldValue.Data.AboutImage1);
        }
        else if (about.AboutImage1 != null)
        {
            about.AboutImage1 = await ImageFileManager.ImageAddAsync(await ImageFileManager.DownloadImageAsync(about.AboutImage1), ContentFileLocations.StaticAboutImageLocation(), ImageResulotions.GetAboutImageResolution());
            if (about.AboutImage1 == null)
            {
                return new ErrorResult(Messages.About1ImageNotUploaded);
            }
            DeleteFileManager.DeleteFile(oldValue.Data.AboutImage1);
        }

        if (about.AboutImage2 == null && aboutImage2 == null)
        {
            about.AboutImage2 = oldValue.Data.AboutImage2;
        }
        else if (aboutImage2 != null)
        {
            about.AboutImage2 = await ImageFileManager.ImageAddAsync(aboutImage2, ContentFileLocations.StaticAboutImageLocation(), ImageResulotions.GetAboutImageResolution());
            if (aboutImage2 == null)
            {
                return new ErrorResult(Messages.About2ImageNotUploaded);
            }
            DeleteFileManager.DeleteFile(oldValue.Data.AboutImage2);
        }
        else if (about.AboutImage2 != null)
        {
            about.AboutImage2 = await ImageFileManager.ImageAddAsync(await ImageFileManager.DownloadImageAsync(about.AboutImage2), ContentFileLocations.StaticAboutImageLocation(), ImageResulotions.GetAboutImageResolution());
            if (aboutImage1 == null)
            {
                return new ErrorResult(Messages.About2ImageNotUploaded);
            }
            DeleteFileManager.DeleteFile(oldValue.Data.AboutImage2);
        }

        await _aboutDal.UpdateAsync(about);

        return new SuccessResult(Messages.AboutUpdated);
    }
}
