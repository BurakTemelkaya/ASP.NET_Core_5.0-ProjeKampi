using BusinessLayer.Abstract;
using CoreDemo.Areas.Admin.Models;
using DataAccessLayer.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class ChartController : Controller
    {

        private readonly ICategoryService _categoryService;

        public ChartController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CategoryChart()
        {
            var categoryList = await _categoryService.GetCategoryandBlogCountAsync();

            List<CategoryModel> list = new();

            foreach (var item in categoryList.Data)
            {
                var categoryCount = categoryList.Data.Count;
                list.Add(new CategoryModel
                {
                    categoryname = item.Category.CategoryName,
                    categorycount = item.CategoryBlogCount
                });
            }

            return Json(new { jsonlist = list });
        }
    }
}
