using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IActionResult> Index(int page = 1, int id = 0)
        {
            var blogs = new List<Blog>();

            if (id != 0)
            {
                var user = await _businessUserService.GetByIDAsync(id.ToString());
                if (user != null)
                {
                    var value = await _blogService.GetBlogListWithCategoryAsync(x => x.WriterID == id);
                    if (value != null)
                        blogs = value.Data;
                    ViewBag.UserName = user.Data.UserName;
                }
            }
            else
            {
                var result = await _blogService.GetBlogListWithCategoryAsync();
                blogs = result.Data;
            }
            blogs = await blogs.OrderByDescending(x => x.BlogCreateDate).ToListAsync();
            var values = await blogs.ToPagedListAsync(page, 4);
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
            var result = await _blogService.BlogAddAsync(blog, User.Identity.Name, blogImage, blogThumbnailImage);
            if (!result.Success)
            {
                ViewBag.CategoryList = await _categoryService.GetCategoryListAsync();
                ModelState.AddModelError("", result.Message);
                return View();
            }           
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> BlogUpdate(int id)
        {
            ViewBag.CategoryList = await _categoryService.GetCategoryListAsync();
            var blogValue = await _blogService.GetBlogByIdForUpdate(id);
            return View(blogValue);
        }

        [HttpPost]
        public async Task<IActionResult> BlogUpdate(Blog blog, IFormFile blogImage, IFormFile blogThumbnailImage)
        {
            await _blogService.BlogAdminUpdateAsync(blog, blogImage, blogThumbnailImage);
            ViewBag.CategoryList = await _categoryService.GetCategoryListAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteBlog(int id)
        {
            var blog = await _blogService.GetFileNameContentBlogByIDAsync(id);
            await _blogService.DeleteBlogByAdminAsync(blog.Data);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ChangeStatusBlog(int id)
        {
            await _blogService.ChangedBlogStatusByAdminAsync(id);
            return RedirectToAction("Index");
        }
    }
}
