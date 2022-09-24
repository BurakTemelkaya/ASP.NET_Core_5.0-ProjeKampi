using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using CoreDemo.Models;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{

    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly UserInfo _userInfo;
        private readonly IBusinessUserService _businessUserService;
        private readonly ICommentService _commentService;

        public BlogController(IBlogService blogService, UserInfo userInfo,
            IBusinessUserService businessUserService, ICommentService commentService)
        {
            _blogService = blogService;
            _userInfo = userInfo;
            _businessUserService = businessUserService;
            _commentService = commentService;
        }

        [AllowAnonymous]
        public IActionResult Index(string id)
        {
            List<BlogandCommentCount> blogandCommentCount = new List<BlogandCommentCount>();
            var values = _blogService.GetBlogListWithCategory();
            var comments = _commentService.GetList();
            if (id != null)
            {
                if (_blogService.GetCount(x => x.CategoryID == Convert.ToInt32(id)) != 0)
                {
                    values.RemoveAll(x => x.CategoryID != Convert.ToInt32(id));
                    ViewBag.id = id;
                }               
            }
            int commentCount = 0;
            foreach (var item in values)
            {
                BlogandCommentCount value = new BlogandCommentCount();
                value.Blog = item;
                foreach (var comment in comments)
                {
                    if (comment.BlogID==item.BlogID)
                    {
                        commentCount++;
                    }                   
                }
                value.ContentCount = commentCount;
                blogandCommentCount.Add(value);
                commentCount = 0;
            }
            return View(blogandCommentCount);
        }
        [AllowAnonymous]
        public IActionResult BlogReadAll(int id)
        {
            ViewBag.i = id;
            var values = _blogService.GetBlogByID(id);
            ViewBag.CommentCount = _commentService.GetCount(x => x.BlogID == id);
            var comments = _commentService.TGetByFilter(x => x.BlogID == id);
            if (comments!=null)
            {
                ViewBag.Star = comments.BlogScore;
            }           
            ViewBag.WriterId = values.FirstOrDefault().WriterID;
            return View(values);
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
        public async Task<IActionResult> BlogAdd(Blog blog)
        {
            var user = await _businessUserService.FindByUserNameAsync(User.Identity.Name);
            if (ModelState.IsValid)
            {
                blog.BlogStatus = true;
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
            CategoryManager cm = new CategoryManager(new EfCategoryRepository());
            List<SelectListItem> CategoryValues = (from x in cm.GetList()
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
            var blogValue = _blogService.TGetByID(id);
            GetCategoryList();
            return View(blogValue);
        }
        [HttpPost]
        public IActionResult EditBlog(Blog blog)
        {
            BlogValidator bv = new BlogValidator();
            ValidationResult results = bv.Validate(blog);
            if (results.IsValid)
            {
                var value = _blogService.TGetByID(blog.BlogID);//eski değeri getirme
                blog.WriterID = _userInfo.GetID(User);
                blog.BlogID = value.BlogID; //frontend kısmından değiştirlmesin diye burda birdaha atama yaptım
                blog.BlogCreateDate = value.BlogCreateDate;//blogCreateDate değişmemesi için tekrar atama yaptım
                _blogService.TUpdate(blog);//update işlemi
                return RedirectToAction("BlogListByWriter");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            GetCategoryList();
            return View();
        }
    }
}
