using BusinessLayer.Abstract;
using BusinessLayer.Constants;
using CoreLayer.Utilities.FileUtilities;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Index()
        {
            var value = await _aboutService.GetAboutByUpdateAsync();
            return View(value.Data);
        }
        [HttpPost]
        public async Task<IActionResult> Index(About about, IFormFile aboutImage1, IFormFile aboutImage2)
        {            
            await _aboutService.UpdateAsync(about,aboutImage1,aboutImage2);
            return View(about);
        }
    }
}
