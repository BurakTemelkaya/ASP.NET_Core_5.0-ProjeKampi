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
            int totalCount = categories.Data.Sum(x => x.CategoryBlogCount);
            foreach (var category in categories.Data)
            {
                var categoryandBlogCount = new CategoryandBlogPercent
                {
                    Category = category.Category,
                    BlogPercent = Math.Round(decimal.Divide(category.CategoryBlogCount, totalCount) * 100).ToString()
                };
                categoryandBlogCounts.Add(categoryandBlogCount);
            }
            ViewBag.TotalBlogCount = totalCount;
            return View(categoryandBlogCounts);
        }
    }
}
