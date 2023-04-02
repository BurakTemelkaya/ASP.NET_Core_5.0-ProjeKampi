using BusinessLayer.Abstract;
using BusinessLayer.ValidationRules;
using CoreLayer.Aspects.AutoFac.Validation;
using CoreLayer.Utilities.Results;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
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

        public async Task<IDataResult<Category>> TGetByIDAsync(int id)
        {
            return new SuccessDataResult<Category>(await _categoryDal.GetByIDAsync(id));
        }

        [ValidationAspect(typeof(CategoryValidator))]
        public async Task<IResult> TAddAsync(Category t)
        {
            t.CategoryStatus = true;
            await _categoryDal.InsertAsync(t);
            return new SuccessResult();
        }

        public async Task<IResult> TDeleteAsync(Category t)
        {
            await _categoryDal.DeleteAsync(t);
            return new SuccessResult();
        }
        [ValidationAspect(typeof(CategoryValidator))]
        public async Task<IResult> TUpdateAsync(Category t)
        {
            await _categoryDal.UpdateAsync(t);
            return new SuccessResult();
        }

        public async Task<IDataResult<List<Category>>> GetListAsync(Expression<Func<Category, bool>> filter = null)
        {
            return new SuccessDataResult<List<Category>>(await _categoryDal.GetListAllAsync(filter));
        }

        public async Task<IDataResult<Category>> TGetByFilterAsync(Expression<Func<Category, bool>> filter = null)
        {
            return new SuccessDataResult<Category>(await _categoryDal.GetByFilterAsync(filter));
        }

        public async Task<IDataResult<int>> GetCountAsync(Expression<Func<Category, bool>> filter = null)
        {
            return new SuccessDataResult<int>(await _categoryDal.GetCountAsync(filter));
        }

        public async Task<IDataResult<List<SelectListItem>>> GetCategoryListAsync()
        {
            var categoryList = await GetListAsync();
            return new SuccessDataResult<List<SelectListItem>>( (from x in categoryList.Data
                    select new SelectListItem
                    {
                        Text = x.CategoryName,
                        Value = x.CategoryID.ToString()
                    }).ToList());
        }

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
    }
}
