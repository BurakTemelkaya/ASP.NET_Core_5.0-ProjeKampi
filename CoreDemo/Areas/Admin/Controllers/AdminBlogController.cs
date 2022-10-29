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
        readonly IBusinessUserService _businessUserService;
        public AdminBlogController(IBlogService blogService, ICategoryService categoryService, IBusinessUserService businessUserService)
        {
            _blogService = blogService;
            _categoryService = categoryService;
            _businessUserService = businessUserService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var blogs = await _blogService.GetBlogListWithCategoryAsync();
            var values = await blogs.ToPagedListAsync(page, 4);
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
        public async Task<IActionResult> BlogAdd()
        {
            ViewBag.CategoryList = await _categoryService.GetCategoryListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> BlogAdd(Blog blog, IFormFile blogImage, IFormFile blogThumbnailImage)
        {
            var value = await _blogService.BlogAddAsync(blog, User.Identity.Name, blogImage, blogThumbnailImage);
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
            ViewBag.CategoryList = _categoryService.GetCategoryListAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> BlogUpdate(int id)
        {
            ViewBag.CategoryList = await _categoryService.GetCategoryListAsync();
            var value = await _blogService.GetBlogByIDAsync(id);
            if (value != null)
                return View(value);
            return RedirectToAction("Index");

        }
        [HttpPost]
        public async Task<IActionResult> BlogUpdate(Blog blog, IFormFile blogImage, IFormFile blogThumbnailImage)
        {
            var blogUser = await _businessUserService.GetByIDAsync(blog.WriterID.ToString());
            var value = await _blogService.BlogAdminUpdateAsync(blog, blogImage, blogThumbnailImage);
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
            ViewBag.CategoryList = _categoryService.GetCategoryListAsync();
            return RedirectToAction("Index");
        }
    }
}
