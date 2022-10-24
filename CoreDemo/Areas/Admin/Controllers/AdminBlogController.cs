using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using X.PagedList;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class AdminBlogController : Controller
    {
        readonly IBlogService _blogService;
        readonly ICategoryService _categoryService;
        public AdminBlogController(IBlogService blogService, ICategoryService categoryService)
        {
            _blogService = blogService;
            _categoryService = categoryService;
        }

        public IActionResult Index(int page = 1)
        {
            var values = _blogService.GetBlogListWithCategory().ToPagedList(page, 4);
            foreach (var item in values)
            {
                if (item.BlogContent.Length > 150)
                {
                    item.BlogContent = item.BlogContent[..130] + "...";
                }
            }
            return View(values);
        }
        [HttpGet]
        public IActionResult BlogAdd()
        {
            ViewBag.CategoryList = _categoryService.GetCategoryList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> BlogAdd(Blog blog, IFormFile blogImage, IFormFile blogThumbnailImage)
        {
            var value = await _blogService.BlogAdd(blog, User.Identity.Name, blogImage, blogThumbnailImage);
            if (value.BlogImage == null)
            {
                ModelState.AddModelError("blogImage", "Lütfen blog resminizin linkini giriniz veya yükleyin.");
                return View(blog);
            }
            if (value.BlogThumbnailImage == null)
            {
                ModelState.AddModelError("blogThumbnailImage", "Lütfen blog küçük resminizin linkini giriniz veya yükleyin.");
                return View(blog);
            }
            ViewBag.CategoryList = _categoryService.GetCategoryList();
            return RedirectToAction("Index");
        }
    }
}
