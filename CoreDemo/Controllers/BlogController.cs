using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using CoreDemo.Models;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
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
        private readonly IWriterService _writerService;
        private readonly UserInfo _userInfo;
        private readonly IBusinessUserService _businessUserService;

        public BlogController(IBlogService blogService, IWriterService writerService, UserInfo userInfo, IBusinessUserService businessUserService)
        {
            _blogService = blogService;
            _writerService = writerService;
            _userInfo = userInfo;
            _businessUserService = businessUserService;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var values = _blogService.GetBlogListWithCategory();
            return View(values);
        }
        [AllowAnonymous]
        public IActionResult BlogReadAll(int id)
        {
            ViewBag.i = id;
            var values = _blogService.GetBlogByID(id);
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
