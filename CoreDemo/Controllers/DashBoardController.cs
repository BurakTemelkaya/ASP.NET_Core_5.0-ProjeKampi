using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    public class DashBoardController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            BlogManager blogManager = new BlogManager(new EfBlogRepository());
            CategoryManager categoryManager = new CategoryManager(new EfCategoryRepository());
            WriterManager writerManager = new WriterManager(new EfWriterRepository());
            string mail = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Email).Value.ToString();
            int id = int.Parse(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Name).Value);
            ViewBag.ToplamBlogSayisi = blogManager.GetList(x => x.BlogStatus == true).Count();
            ViewBag.YazarinBlogSayisi = blogManager.GetBlogByWriter(id).Count();
            ViewBag.KategoriSayisi = categoryManager.GetList().Count();
            return View();
        }
    }
}
