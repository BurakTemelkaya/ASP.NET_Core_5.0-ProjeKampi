using CoreLayer.Utilities.Results;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract;

public interface ICategoryService
{
    Task<IResultObject> TAddAsync(Category t);
    Task<IResultObject> TDeleteAsync(Category t);
    Task<IResultObject> TUpdateAsync(Category t);
    Task<IDataResult<List<Category>>> GetListAsync(bool? categoryStatus=null);
    Task<IDataResult<List<Category>>> GetListByPagingAsync(bool? categoryStatus=null, int take = 0, int page = 0);
    Task<IDataResult<Category>> TGetByIDAsync(int id);
    Task<IDataResult<int>> GetCountAsync(bool? categoryStatus=null);
    Task<IDataResult<List<SelectListItem>>> GetCategorySelectedListItemAsync(bool? isActive = null);
    Task<IResultObject> ChangedStatusAsync(int id);
    Task<IDataResult<List<CategoryBlogandBlogCountDto>>> GetCategoryandBlogCountAsync();
}
