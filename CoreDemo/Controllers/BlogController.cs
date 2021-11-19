using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    [AllowAnonymous]
    public class BlogController : Controller
    {
        BlogManager bm = new BlogManager(new EfBlogRepository());
        WriterManager wm = new WriterManager(new EfWriterRepository());
        public IActionResult Index()
        {
            var values = bm.GetBlogListWithCategory();
            return View(values);
        }
        public IActionResult BlogReadAll(int id)
        {
            ViewBag.i = id;
            var values = bm.GetBlogByID(id);
            return View(values);
        }
        public IActionResult BlogListByWriter()
        {
            int id = wm.TGetByFilter(x => x.WriterMail== User.Identity.Name).WriterID;
            var values = bm.GetListWithCategoryByWriterBm(id);
            return View(values);
        }
        [HttpGet]
        public IActionResult BlogAdd()
        {
            GetCategoryList();
            return View();
        }
        [HttpPost]
        public IActionResult BlogAdd(Blog blog)
        {
            BlogValidator bv = new BlogValidator();
            ValidationResult results = bv.Validate(blog);
            if (results.IsValid)
            {
                blog.BlogStatus = true;
                blog.BlogCreateDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                blog.WriterID = wm.TGetByFilter(x => x.WriterMail == User.Identity.Name).WriterID;
                bm.TAdd(blog);
                return RedirectToAction("BlogListByWriter", "Blog");
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
            var blogValue = bm.TGetByID(id);
            bm.TDelete(blogValue);
            Thread.Sleep(2000);
            return RedirectToAction("BlogListByWriter");
        }
        public IActionResult ChangeStatusBlog(int id)
        {
            var blogValue = bm.TGetByID(id);
            if (blogValue.BlogStatus)
            {
                blogValue.BlogStatus = false;
            }
            else
            {
                blogValue.BlogStatus = true;
            }
            bm.TUpdate(blogValue);
            Thread.Sleep(2000);
            return RedirectToAction("BlogListByWriter");
        }
        [HttpGet]
        public IActionResult EditBlog(int id)
        {
            var blogValue = bm.TGetByID(id);
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
                var value = bm.TGetByID(blog.BlogID);//eski değeri getirme
                blog.WriterID = wm.TGetByFilter(x => x.WriterMail == User.Identity.Name).WriterID;
                blog.BlogID = value.BlogID; //frontend kısmından değiştirlmesin diye burda birdaha atama yaptım
                blog.BlogCreateDate = value.BlogCreateDate;//blogCreateDate değişmemesi için tekrar atama yaptım
                bm.TUpdate(blog);//update işlemi
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
