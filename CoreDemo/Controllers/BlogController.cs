﻿using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using CoreDemo.Models;
using DataAccessLayer.EntityFramework;
using DocumentFormat.OpenXml.Office2010.Excel;
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
        public IActionResult Index(string id, int page = 1, string search = null)
        {
            ViewData["Title"] = "Ana Sayfa";
            List<Blog> values = new();
            List<BlogandCommentCount> blogandCommentCount = new();
            if (id == null)
            {
                values = _blogService.GetBlogListWithCategory().Where(x => x.BlogStatus)
                    .OrderByDescending(x => x.BlogCreateDate).ToList();
            }
            if (id != null)
            {
                if (_blogService.GetCount(x => x.CategoryID == Convert.ToInt32(id)) != 0)
                {
                    values = _blogService.GetBlogListWithCategory()
                        .Where(x => x.BlogStatus && x.CategoryID == Convert.ToInt32(id))
                        .OrderByDescending(x => x.BlogCreateDate).ToList();
                    values.RemoveAll(x => x.CategoryID != Convert.ToInt32(id));
                    ViewBag.id = id;
                    ViewData["Title"] = values.FirstOrDefault().Category.CategoryName + " Blogları";
                }
                else
                {
                    values = _blogService.GetBlogListWithCategory().OrderByDescending(x => x.BlogCreateDate).ToList();
                }
            }
            if (search != null)
            {
                values = values.Where(x => x.BlogTitle.ToLower().Contains(search.ToLower())).ToList();
                ViewBag.Message = "'" + search + "' aramanız dair sonuçlar.";
                ViewData["Title"] = search;
                ViewBag.Sonuc = true;
                if (values.Count < 1)
                {
                    values = null;
                    values = _blogService.GetBlogListWithCategory().ToList();
                    ViewBag.Message = "'" + search + "' aramanıza dair sonuç bulunamadı.";
                    ViewBag.Sonuc = false;
                }
            }
            var comments = _commentService.GetList();
            int commentCount = 0;
            foreach (var item in values)
            {
                BlogandCommentCount value = new BlogandCommentCount();
                value.Blog = item;
                foreach (var comment in comments)
                {
                    if (comment.BlogID == item.BlogID)
                        commentCount++;
                }
                value.ContentCount = commentCount;
                blogandCommentCount.Add(value);
                commentCount = 0;
            }
            return View(blogandCommentCount.ToPagedList(page, 6));
        }
        [AllowAnonymous]
        public async Task<IActionResult> BlogReadAll(int id)
        {
            ViewBag.i = id;
            var value = _blogService.GetBlogByID(id);
            if (value == null)
            {
                return RedirectToAction("Error404", "ErrorPage");
            }
            ViewBag.CommentCount = _commentService.GetCount(x => x.BlogID == id);
            var comments = _commentService.TGetByFilter(x => x.BlogID == id);
            if (comments != null)
            {
                ViewBag.Star = comments.BlogScore;
            }
            var writer = await _businessUserService.GetByIDAsync(value.WriterID.ToString());
            ViewBag.WriterId = writer.Id;
            ViewBag.WriterName = writer.NameSurname;
            ViewData["Title"] = value.BlogTitle;
            return View(value);
        }
        public async Task<IActionResult> BlogListByWriter()
        {
            var userName = User.Identity.Name;
            var user = await _businessUserService.FindByUserNameAsync(userName);
            var values = _blogService.GetListWithCategoryByWriterBm(user.Id);
            return View(values);
        }
        [HttpGet]
        public IActionResult BlogAdd()
        {
            GetCategoryList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> BlogAdd(Blog blog, IFormFile blogImage, IFormFile blogThumbnailImage)
        {
            var user = await _businessUserService.FindByUserNameAsync(User.Identity.Name);
            if (ModelState.IsValid)
            {
                if (blogImage != null)
                    blog.BlogImage = AddImage.ImageAdd(blogImage, AddImage.StaticProfileImageLocation());
                else if (blog.BlogImage == null)
                    ModelState.AddModelError("blogImage", "Lütfen blog resminizin linkini giriniz veya yükleyin.");
                if (blogThumbnailImage != null)
                    blog.BlogThumbnailImage = AddImage.ImageAdd(blogThumbnailImage, AddImage.StaticProfileImageLocation());
                else if (blog.BlogThumbnailImage == null)
                    ModelState.AddModelError("blogThumbnailImage", "Lütfen blog küçük resminizin linkini giriniz veya yükleyin.");
                blog.BlogCreateDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                blog.WriterID = user.Id;
                _blogService.TAdd(blog);
                return RedirectToAction("BlogListByWriter", "Blog");
            }
            GetCategoryList();
            return View();
        }
        public void GetCategoryList()
        {
            List<SelectListItem> CategoryValues = (from x in _categoryService.GetList()
                                                   select new SelectListItem
                                                   {
                                                       Text = x.CategoryName,
                                                       Value = x.CategoryID.ToString()
                                                   }).ToList();
            ViewBag.CategoryList = CategoryValues;
        }
        public IActionResult DeleteBlog(int id)
        {
            var blogValue = _blogService.TGetByID(id);
            _blogService.TDelete(blogValue);
            Thread.Sleep(2000);
            return RedirectToAction("BlogListByWriter");
        }
        public IActionResult ChangeStatusBlog(int id)
        {
            var blogValue = _blogService.TGetByID(id);
            if (blogValue.BlogStatus)
            {
                blogValue.BlogStatus = false;
            }
            else
            {
                blogValue.BlogStatus = true;
            }
            _blogService.TUpdate(blogValue);
            Thread.Sleep(2000);
            return RedirectToAction("BlogListByWriter");
        }
        [HttpGet]
        public IActionResult EditBlog(int id)
        {
            if (id != 0)
            {
                var blogValue = _blogService.TGetByID(id);
                GetCategoryList();
                if (blogValue.BlogImage.Substring(0, 5) != "https" || blogValue.BlogImage.Substring(0, 4) != "http")
                    blogValue.BlogImage = null;
                if (blogValue.BlogThumbnailImage.Substring(0, 5) != "https" || blogValue.BlogThumbnailImage.Substring(0, 4) != "http")
                    blogValue.BlogThumbnailImage = null;
                return View(blogValue);
            }
            return RedirectToAction("Error404", "ErrorPage");
        }
        [HttpPost]
        public async Task<IActionResult> EditBlog(Blog blog, IFormFile blogImage, IFormFile blogThumbnailImage)
        {
            BlogValidator bv = new BlogValidator();
            ValidationResult results = bv.Validate(blog);
            var oldValue = _blogService.TGetByID(blog.BlogID);
            if (results.IsValid)
            {
                if (blogImage != null)
                    blog.BlogImage = AddImage.ImageAdd(blogImage, AddImage.StaticProfileImageLocation());
                else if (blog.BlogImage == null)
                    blog.BlogImage = oldValue.BlogImage;
                if (blogThumbnailImage != null)
                    blog.BlogThumbnailImage = AddImage.ImageAdd(blogThumbnailImage, AddImage.StaticProfileImageLocation());
                else if (blog.BlogThumbnailImage == null)
                    blog.BlogThumbnailImage = oldValue.BlogThumbnailImage;
                var user = await _businessUserService.FindByUserNameAsync(User.Identity.Name);
                var value = _blogService.GetListWithCategoryByWriterBm(user.Id);//eski değeri getirme
                if (!value.Any(x => x.WriterID == user.Id && x.BlogID == blog.BlogID))
                {
                    return RedirectToAction("BlogListByWriter");
                }
                blog.BlogCreateDate = value.FirstOrDefault(x => x.BlogID == blog.BlogID).BlogCreateDate;//blogCreateDate değişmemesi için tekrar atama yaptım
                blog.Writer = user;
                _blogService.TUpdate(blog);//update işlemi
                return RedirectToAction("BlogListByWriter");
            }
            GetCategoryList();
            return View();
        }
    }
}
