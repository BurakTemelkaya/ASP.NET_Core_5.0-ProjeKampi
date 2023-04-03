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
        private readonly IBlogService _blogService;

        public ChartController(ICategoryService categoryService, IBlogService blogService)
        {
            _categoryService = categoryService;
            _blogService = blogService;

        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CategoryChart()
        {
            var categoryList = await _categoryService.GetListAsync();

            List<CategoryModel> list = new();

            foreach (var item in categoryList.Data)
            {
                var categoryCount = await _blogService.GetCountAsync(x => x.CategoryID == item.CategoryID);
                list.Add(new CategoryModel
                {
                    categoryname = item.CategoryName,
                    categorycount = categoryCount.Data
                });
            }

            return Json(new { jsonlist = list });
        }
    }
}
