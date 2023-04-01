using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using CoreDemo.Models;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using X.PagedList;

namespace CoreDemo.Controllers
{

    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly IBusinessUserService _businessUserService;
        private readonly ICommentService _commentService;
        private readonly ICategoryService _categoryService;

        public BlogController(IBlogService blogService,
            IBusinessUserService businessUserService, ICommentService commentService, ICategoryService categoryService)
        {
            _blogService = blogService;
            _businessUserService = businessUserService;
            _commentService = commentService;
            _categoryService = categoryService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string id, int page = 1, string search = null)
        {
            var values = await _blogService.GetBlogListByMainPage(id, page, search);

            ViewData["Title"] = "Ana Sayfa";

            if (id != null || search != null)
            {
                ViewData["Title"] = values.Message;
                ViewBag.Message = values.Message;
                ViewBag.IsSuccess = values.Success;
            }

            return View(await values.Data.ToPagedListAsync(page, 6));
        }
        [AllowAnonymous]
        public async Task<IActionResult> BlogReadAll(int id)
        {
            ViewBag.i = id;
            var value = await _blogService.GetBlogByIDAsync(id);
            if (value == null || !value.Data.BlogStatus)
            {
                return RedirectToAction("Error404", "ErrorPage");
            }
            ViewBag.CommentCount = _commentService.GetCountAsync(x => x.BlogID == id).Result.Data;
            var comments = await _commentService.TGetByFilterAsync(x => x.BlogID == id);
            if (comments.Data != null)
            {
                ViewBag.Star = comments.Data.BlogScore;
            }
            var writer = await _businessUserService.GetByIDAsync(value.Data.WriterID.ToString());
            ViewBag.WriterId = writer.Data.Id;
            ViewBag.WriterName = writer.Data.NameSurname;
            ViewData["Title"] = value.Data.BlogTitle;
            return View(value.Data);
        }
        public async Task<IActionResult> BlogListByWriter(int page = 1)
        {
            var values = await _blogService.GetListWithCategoryByWriterBmAsync(User.Identity.Name);
            return View(await values.Data.ToPagedListAsync(page, 5));
        }
        [HttpGet]
        public async Task<IActionResult> BlogAdd()
        {
            var categoryList = await _categoryService.GetCategoryListAsync();
            ViewBag.CategoryList = categoryList.Data;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> BlogAdd(Blog blog, IFormFile blogImage, IFormFile blogThumbnailImage)
        {
            var value = await _blogService.BlogAddAsync(blog, User.Identity.Name, blogImage, blogThumbnailImage);
            var categoryList = await _categoryService.GetCategoryListAsync();
            ViewBag.CategoryList = categoryList.Data;
            if (!value.Success)
            {
                ModelState.AddModelError("blogImage", value.Message);
                return View(blog);
            }
            return RedirectToAction("BlogListByWriter", "Blog");
        }
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var blogValue = await _blogService.GetBlogByIDAsync(id);
            await _blogService.DeleteBlogAsync(blogValue.Data, User.Identity.Name);
            return RedirectToAction("BlogListByWriter");
        }
        public async Task<IActionResult> ChangeStatusBlog(int id)
        {
            await _blogService.ChangedBlogStatusAsync(id, User.Identity.Name);
            return RedirectToAction("BlogListByWriter");
        }
        [HttpGet]
        public async Task<IActionResult> EditBlog(int id)
        {
            var blogValue = await _blogService.GetBlogByIdForUpdate(id);
            ViewBag.CategoryList = await _categoryService.GetCategoryListAsync();
            return View(blogValue.Data);
        }
        [HttpPost]
        public async Task<IActionResult> EditBlog(Blog blog, IFormFile blogImage, IFormFile blogThumbnailImage)
        {
            await _blogService.BlogUpdateAsync(blog, User.Identity.Name, blogImage, blogThumbnailImage);
            ViewBag.CategoryList = await _categoryService.GetCategoryListAsync();
            return RedirectToAction("BlogListByWriter");
        }
    }
}
