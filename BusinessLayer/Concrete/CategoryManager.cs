using BusinessLayer.Abstract;
using BusinessLayer.ValidationRules;
using CoreDemo.Models;
using CoreLayer.Aspects.AutoFac.Caching;
using CoreLayer.Aspects.AutoFac.Validation;
using CoreLayer.Utilities.Results;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
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
        public async Task<IResult> TAddAsync(Category t)
        {
            t.CategoryStatus = true;
            await _categoryDal.InsertAsync(t);
            return new SuccessResult();
        }

        [CacheRemoveAspect("IBlogService.Get")]
        [CacheRemoveAspect("ICategoryService.Get")]
        public async Task<IResult> TDeleteAsync(Category t)
        {
            await _categoryDal.DeleteAsync(t);
            return new SuccessResult();
        }

        [CacheRemoveAspect("IBlogService.Get")]
        [CacheRemoveAspect("ICategoryService.Get")]
        [ValidationAspect(typeof(CategoryValidator))]
        public async Task<IResult> TUpdateAsync(Category t)
        {
            await _categoryDal.UpdateAsync(t);
            return new SuccessResult();
        }

        [CacheAspect]
        public async Task<IDataResult<List<Category>>> GetListAsync(Expression<Func<Category, bool>> filter = null)
        {
            return new SuccessDataResult<List<Category>>(await _categoryDal.GetListAllAsync(filter));
        }

        [CacheAspect]
        public async Task<IDataResult<Category>> TGetByFilterAsync(Expression<Func<Category, bool>> filter = null)
        {
            return new SuccessDataResult<Category>(await _categoryDal.GetByFilterAsync(filter));
        }

        [CacheAspect]
        public async Task<IDataResult<int>> GetCountAsync(Expression<Func<Category, bool>> filter = null)
        {
            return new SuccessDataResult<int>(await _categoryDal.GetCountAsync(filter));
        }

        [CacheAspect]
        public async Task<IDataResult<List<SelectListItem>>> GetCategorySelectedListItemAsync()
        {
            var categoryList = await GetListAsync();
            return new SuccessDataResult<List<SelectListItem>>((from x in categoryList.Data
                                                                select new SelectListItem
                                                                {
                                                                    Text = x.CategoryName,
                                                                    Value = x.CategoryID.ToString()
                                                                }).ToList());
        }

        [CacheRemoveAspect("IBlogService.Get")]
        [CacheRemoveAspect("ICategoryService.Get")]
        public async Task<IResult> ChangedStatusAsync(int id)
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
            return new ErrorResult("İşlem başarısız.");
        }

        [CacheAspect]
        public async Task<IDataResult<List<CategoryBlogandBlogCountDto>>> GetCategoryandBlogCountAsync()
        {
            var values = await _categoryDal.GetListWithCategoryByBlog(x => x.CategoryStatus);
            var blogCategoryCount = new List<CategoryBlogandBlogCountDto>();
            foreach (var value in values)
            {
                var categoryandBlogCount = new CategoryBlogandBlogCountDto();
                categoryandBlogCount.Category = value;
                categoryandBlogCount.CategoryBlogCount = value.Blogs.Count(x=> x.BlogStatus);
                blogCategoryCount.Add(categoryandBlogCount);
            }
            return new SuccessDataResult<List<CategoryBlogandBlogCountDto>>(blogCategoryCount);
        }
    }
}
