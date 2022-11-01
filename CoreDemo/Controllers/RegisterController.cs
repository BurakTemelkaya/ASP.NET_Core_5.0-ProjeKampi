using BusinessLayer.Abstract;
using CoreDemo.Models;
using DocumentFormat.OpenXml.Vml;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
            UserSignUpViewModel signUpViewModel = new UserSignUpViewModel
            {
                Cities = await _writerCity.GetCityListAsync()
            };
            return View(signUpViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Index(UserSignUpViewModel signUpViewModel)
        {
            if (!signUpViewModel.IsAcceptTheContract)
            {
                ModelState.AddModelError("IsAcceptTheContract",
                    "Sayfamıza kayıt olabilmek için gizlilik sözleşmesini kabul etmeniz gerekmektedir.");
                signUpViewModel.Cities = await _writerCity.GetCityListAsync();
                return View(signUpViewModel);
            }
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser()
                {
                    Email = signUpViewModel.Mail,
                    UserName = signUpViewModel.UserName,
                    NameSurname = signUpViewModel.NameSurname,
                    About = signUpViewModel.About,
                    City = signUpViewModel.City
                };
                if (signUpViewModel.ImageFile != null)
                {
                    user.ImageUrl = AddImage.ImageAdd(signUpViewModel.ImageFile, AddImage.StaticProfileImageLocation());
                }
                else if (signUpViewModel.ImageUrl != null)
                {
                    user.ImageUrl = signUpViewModel.ImageUrl;
                }
                await _userService.RegisterUserAsync(user, signUpViewModel.Password);
                await _signInManager.SignInAsync(user, true);
                return RedirectToAction("Index", "Dashboard");
            }
            signUpViewModel.Cities = await _writerCity.GetCityListAsync();
            return View(signUpViewModel);
        }
    }
}