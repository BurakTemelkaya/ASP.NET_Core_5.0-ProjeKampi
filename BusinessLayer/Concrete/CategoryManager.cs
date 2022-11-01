using BusinessLayer.Abstract;
using BusinessLayer.ValidationRules;
using CoreLayer.Aspects.AutoFac.Validation;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using DataAccessLayer.Repositories;
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

        public async Task<Category> TGetByIDAsync(int id)
        {
            return await _categoryDal.GetByIDAsync(id);
        }
        [ValidationAspect(typeof(CategoryValidator))]
        public async Task TAddAsync(Category t)
        {
            t.CategoryStatus = true;
            await _categoryDal.InsertAsync(t);
        }

        public async Task TDeleteAsync(Category t)
        {
            await _categoryDal.DeleteAsync(t);
        }
        [ValidationAspect(typeof(CategoryValidator))]
        public async Task TUpdateAsync(Category t)
        {
            await _categoryDal.UpdateAsync(t);
        }

        public async Task<List<Category>> GetListAsync(Expression<Func<Category, bool>> filter = null)
        {
            return filter == null ?
                await _categoryDal.GetListAllAsync() :
                await _categoryDal.GetListAllAsync(filter);
        }

        public async Task<Category> TGetByFilterAsync(Expression<Func<Category, bool>> filter = null)
        {
            return filter == null ?
                await _categoryDal.GetByFilterAsync() :
                await _categoryDal.GetByFilterAsync(filter);
        }

        public async Task<int> GetCountAsync(Expression<Func<Category, bool>> filter = null)
        {
            return await _categoryDal.GetCountAsync(filter);
        }

        public async Task<List<SelectListItem>> GetCategoryListAsync()
        {
            return (from x in await GetListAsync()
                    select new SelectListItem
                    {
                        Text = x.CategoryName,
                        Value = x.CategoryID.ToString()
                    }).ToList();
        }

        public async Task ChangedStatusAsync(int id)
        {
            var value = await TGetByIDAsync(id);
            if (value.CategoryStatus)
                value.CategoryStatus = false;
            else
                value.CategoryStatus = true;
            await TUpdateAsync(value);
        }
    }
}
