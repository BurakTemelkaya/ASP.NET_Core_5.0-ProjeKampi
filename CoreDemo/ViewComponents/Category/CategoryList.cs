using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using CoreDemo.Models;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.ViewComponents.Category
{
    public class CategoryList : ViewComponent
    {
        private readonly ICategoryService _categoryService;
        private readonly IBlogService _blogService;

        public CategoryList(ICategoryService categoryService, IBlogService blogService)
        {
            _categoryService = categoryService;
            _blogService = blogService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _categoryService.GetListAsync(x=> x.CategoryStatus);
            var blogs = await _blogService.GetListAsync(x=> x.BlogStatus);
            var blogCategoryCount = new List<CategoryBlogandBlogCount>();
            int categoryCount = 0;
            foreach (var value in values)
            {
                var categoryandBlogCount = new CategoryBlogandBlogCount();
                categoryandBlogCount.Categorys = value;
                foreach (var blog in blogs)
                {
                    if (value.CategoryID == blog.CategoryID)
                    {
                        categoryCount++;
                    }
                }
                categoryandBlogCount.CategoryCount = categoryCount;                
                categoryCount = 0;
                blogCategoryCount.Add(categoryandBlogCount);
            }
            return View(blogCategoryCount);
        }
    }
}
