using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using CoreDemo.Models;
using DataAccessLayer.EntityFramework;
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
        private readonly IBlogService _blogService;
        public CategoryListDashboard(ICategoryService categoryService, IBlogService blogService)
        {
            _categoryService = categoryService;
            _blogService = blogService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categorys = await _categoryService.GetListAsync();
            var blogs = await _blogService.GetListAsync();
            var categoryandBlogCounts = new List<CategoryandBlogPercent>();
            int blogCount = 0;
            foreach (var category in categorys.Data)
            {
                var categoryandBlogCount = new CategoryandBlogPercent();
                categoryandBlogCount.Category = category;
                foreach (var blog in blogs.Data)
                {
                    if (category.CategoryID == blog.CategoryID)
                    {
                        blogCount++;
                    }
                }
                categoryandBlogCount.BlogPercent = Math.Round(decimal.Divide(blogCount, blogs.Data.Count) * 100).ToString();
                blogCount = 0;
                categoryandBlogCounts.Add(categoryandBlogCount);
            }
            ViewBag.TotalBlogCount = blogs.Data.Count;
            return View(categoryandBlogCounts);
        }
    }
}
