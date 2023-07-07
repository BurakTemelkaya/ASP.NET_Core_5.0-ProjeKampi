using CoreLayer.Utilities.Results;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface ICategoryService
    {
        Task<IResult> TAddAsync(Category t);
        Task<IResult> TDeleteAsync(Category t);
        Task<IResult> TUpdateAsync(Category t);
        Task<IDataResult<List<Category>>> GetListAsync(Expression<Func<Category, bool>> filter = null);
        Task<IDataResult<Category>> TGetByIDAsync(int id);
        Task<IDataResult<Category>> TGetByFilterAsync(Expression<Func<Category, bool>> filter = null);
        Task<IDataResult<int>> GetCountAsync(Expression<Func<Category, bool>> filter = null);
        Task<IDataResult<List<SelectListItem>>> GetCategorySelectedListItemAsync(bool? isActive = null);
        Task<IResult> ChangedStatusAsync(int id);
        Task<IDataResult<List<CategoryBlogandBlogCountDto>>> GetCategoryandBlogCountAsync();
    }
}
