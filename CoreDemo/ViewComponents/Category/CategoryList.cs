using BusinessLayer.Abstract;
using CoreDemo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreDemo.ViewComponents.Category
{
    public class CategoryList : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public CategoryList(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _categoryService.GetCategoryandBlogCountAsync();
            return View(result.Data);
        }
    }
}
