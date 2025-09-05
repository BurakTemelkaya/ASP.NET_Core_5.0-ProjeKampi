using BusinessLayer.Abstract;
using BusinessLayer.ValidationRules;
using CoreLayer.Aspects.AutoFac.Caching;
using CoreLayer.Aspects.AutoFac.Validation;
using CoreLayer.Extensions;
using CoreLayer.Utilities.Results;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using X.PagedList;
using X.PagedList.EF;

namespace BusinessLayer.Concrete;

public class CategoryManager : ICategoryService
{
    private readonly ICategoryDal _categoryDal;

    public CategoryManager(ICategoryDal categoryDal)
    {
        _categoryDal = categoryDal;
    }

    [CacheAspect]
    public async Task<IDataResult<Category>> TGetByIDAsync(int id)
    {
        return new SuccessDataResult<Category>(await _categoryDal.GetByIDAsync(id));
    }

    [CacheRemoveAspect("IBlogService.Get")]
    [CacheRemoveAspect("ICategoryService.Get")]
    [ValidationAspect(typeof(CategoryValidator))]
    public async Task<IResultObject> TAddAsync(Category t)
    {
        t.CategoryStatus = true;
        await _categoryDal.InsertAsync(t);
        return new SuccessResult();
    }

    [CacheRemoveAspect("IBlogService.Get")]
    [CacheRemoveAspect("ICategoryService.Get")]
    public async Task<IResultObject> TDeleteAsync(Category t)
    {
        await _categoryDal.DeleteAsync(t);
        return new SuccessResult();
    }

    [CacheRemoveAspect("IBlogService.Get")]
    [CacheRemoveAspect("ICategoryService.Get")]
    [ValidationAspect(typeof(CategoryValidator))]
    public async Task<IResultObject> TUpdateAsync(Category t)
    {
        await _categoryDal.UpdateAsync(t);
        return new SuccessResult();
    }

    [CacheAspect]
    public async Task<IDataResult<List<Category>>> GetListAsync(bool? categoryStatus = null)
    {
        return new SuccessDataResult<List<Category>>(categoryStatus == null ? await _categoryDal.GetListAllAsync()
            : await _categoryDal.GetListAllAsync(x => x.CategoryStatus == categoryStatus));
    }

    [CacheAspect]
    public async Task<IDataResult<IPagedList<Category>>> GetListByPagingAsync(bool? categoryStatus = null, int take = 0, int page = 0)
    {
        Expression<Func<Category, bool>> predicate = null;

        if (categoryStatus.HasValue)
        {
            predicate = predicate.And(x => x.CategoryStatus == categoryStatus);
        }

        var pageList = await _categoryDal.GetPagedListAsync(page, take, predicate);

        return new SuccessDataResult<IPagedList<Category>>(pageList);
    }


    [CacheAspect]
    public async Task<IDataResult<int>> GetCountAsync(bool? categoryStatus = null)
    {
        return new SuccessDataResult<int>(categoryStatus == null ? await _categoryDal.GetCountAsync()
            : await _categoryDal.GetCountAsync(x => x.CategoryStatus == categoryStatus));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="isActive">Boş verilirse bütün değerler gelir. True yada False değeri verilirse,
    /// bu değere göre aktif yada pasif kategoriler gelir.</param>
    /// <returns></returns>
    [CacheAspect]
    public async Task<IDataResult<List<SelectListItem>>> GetCategorySelectedListItemAsync(bool? isActive = null)
    {
        var categoryList = isActive == null ? await GetListAsync()
            : await GetListAsync(true);

        return new SuccessDataResult<List<SelectListItem>>((from x in categoryList.Data
                                                            select new SelectListItem
                                                            {
                                                                Text = x.CategoryName,
                                                                Value = x.CategoryID.ToString()
                                                            }).ToList());
    }

    [CacheRemoveAspect("IBlogService.Get")]
    [CacheRemoveAspect("ICategoryService.Get")]
    public async Task<IResultObject> ChangedStatusAsync(int id)
    {
        var value = await TGetByIDAsync(id);
        if (value.Data.CategoryStatus)
            value.Data.CategoryStatus = false;
        else
            value.Data.CategoryStatus = true;
        var result = await TUpdateAsync(value.Data);
        if (result.Success)
        {
            return new SuccessResult();
        }
        return new ErrorResult(result.Message);
    }

    [CacheAspect]
    public async Task<IDataResult<List<CategoryBlogandBlogCountDto>>> GetCategoryandBlogCountAsync()
    {
        var values = await _categoryDal.GetListWithCategoryByBlog(x => x.CategoryStatus && x.BlogStatus);
        return new SuccessDataResult<List<CategoryBlogandBlogCountDto>>(values);
    }
}
