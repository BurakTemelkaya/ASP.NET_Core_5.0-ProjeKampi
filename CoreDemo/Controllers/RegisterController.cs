using BusinessLayer.Abstract;
using CoreDemo.Models;
using CoreLayer.Utilities.FileUtilities;
using DocumentFormat.OpenXml.Vml;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        private readonly IBusinessUserService _userService;
        private readonly WriterCity _writerCity;
        private readonly SignInManager<AppUser> _signInManager;

        public RegisterController(IBusinessUserService userService, WriterCity writerCity, SignInManager<AppUser> signInManager)
        {
            _userService = userService;
            _writerCity = writerCity;
            _signInManager = signInManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (User.Identity.Name != null)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            ViewBag.Cities = await _writerCity.GetCityListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(UserSignUpDto userSignUpDto)
        {
            if (!userSignUpDto.IsAcceptTheContract)
            {
                ModelState.AddModelError("IsAcceptTheContract",
                    "Sayfamıza kayıt olabilmek için gizlilik sözleşmesini kabul etmeniz gerekmektedir.");
                ViewBag.Cities = await _writerCity.GetCityListAsync();
                return View(userSignUpDto);
            }
            var result = await _userService.RegisterUserAsync(userSignUpDto, userSignUpDto.Password);
            if (result == null)
            {
                var user = await _userService.FindByUserNameAsync(userSignUpDto.UserName);
                await _signInManager.SignInAsync(user, true);
                return RedirectToAction("Index", "Dashboard");
            }
            ModelState.AddModelError("Username",
                    "Bir hata oluştu lütfen girdiğiniz değerleri kontrol edin.");
            ViewBag.Cities = await _writerCity.GetCityListAsync();
            return View(userSignUpDto);
        }
    }
}