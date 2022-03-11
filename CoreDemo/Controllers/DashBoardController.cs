using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly IBlogService _blogService;
        private readonly ICategoryService _categoryService;
        private readonly IUserService _userService;

        public DashBoardController(IBlogService blogService, ICategoryService categoryService, IUserService userService)
        {
            _blogService = blogService;
            _categoryService = categoryService;
            _userService = userService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var userName = User.Identity.Name;
            var user = await _userService.GetByUserNameAsync(userName);
            ViewBag.ToplamBlogSayisi = _blogService.GetCount(x => x.BlogStatus == true);
            ViewBag.YazarinBlogSayisi = _blogService.GetCount(x => x.WriterID == user.Id);
            ViewBag.KategoriSayisi = _categoryService.GetCount(x => x.CategoryStatus == true);
            return View();
        }
    }
}
