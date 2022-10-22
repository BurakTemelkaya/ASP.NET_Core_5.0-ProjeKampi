using BusinessLayer.Abstract;
using CoreDemo.Models;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AboutController : Controller
    {
        readonly IAboutService _aboutService;
        public AboutController(IAboutService aboutService)
        {
            _aboutService = aboutService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var value = _aboutService.TGetByFilter();
            return View(value);
        }
        [HttpPost]
        public IActionResult Index(About about, IFormFile aboutImage1, IFormFile aboutImage2)
        {
            if (about.AboutImage1 == null && aboutImage1 != null)
                about.AboutImage1= AddImage.ImageAdd(aboutImage1,AddImage.StaticAboutImageLocation());
            if (about.AboutImage2 == null && aboutImage2 != null)
                about.AboutImage2 = AddImage.ImageAdd(aboutImage2, AddImage.StaticAboutImageLocation());
            if (about.AboutImage1==null || about.AboutImage2==null)
            {
                ModelState.AddModelError("AboutImage1", "Lütfen Hiçbir Resmi Boş Bırakmayınız");
                return View(about);
            }
            _aboutService.TUpdate(about);
            return View();
        }
    }
}
