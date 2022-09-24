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
        public IViewComponentResult Invoke()
        {
            var categorys = _categoryService.GetList();
            var blogs = _blogService.GetList();
            var categoryandBlogCounts = new List<CategoryandBlogPercent>();
            int blogCount = 0;
            foreach (var category in categorys)
            {
                var categoryandBlogCount = new CategoryandBlogPercent();
                categoryandBlogCount.Category = category;
                foreach (var blog in blogs)
                {
                    if (category.CategoryID == blog.CategoryID)
                    {
                        blogCount++;
                    }
                }
                categoryandBlogCount.BlogPercent = Math.Round((decimal.Divide(blogCount, blogs.Count)) * 100);
                blogCount = 0;
                categoryandBlogCounts.Add(categoryandBlogCount);
            }
            ViewBag.TotalBlogCount = blogs.Count;
            return View(categoryandBlogCounts);
        }
    }
}
