using BusinessLayer.Abstract;
using CoreDemo.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.ViewComponents.Category
{
    public class CategoryListDashboard : ViewComponent
    {
        private readonly ICategoryService _categoryService;
        public CategoryListDashboard(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _categoryService.GetCategoryandBlogCountAsync();
            var categoryandBlogCounts = new List<CategoryandBlogPercent>();
            int totalCount = categories.Data.Sum(x => x.NumberofBloginCategory);
            foreach (var category in categories.Data)
            {
                var categoryandBlogCount = new CategoryandBlogPercent
                {
                    CategoryName = category.CategoryName,
                    CategoryDescription = category.CategoryDescription,
                    BlogPercent = Math.Round(decimal.Divide(category.NumberofBloginCategory, totalCount) * 100).ToString()
                };
                categoryandBlogCounts.Add(categoryandBlogCount);
            }
            ViewBag.TotalBlogCount = totalCount;
            return View(categoryandBlogCounts);
        }
    }
}
