using BusinessLayer.Abstract;
using CoreDemo.Models;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    [AllowAnonymous]
    public class RegisterUserController : Controller
    {
        private readonly IBusinessUserService _userService;
        private readonly WriterCity _writerCity;

        public RegisterUserController(IBusinessUserService userService, WriterCity writerCity)
        {
            userService = _userService;
            _writerCity = writerCity;
        }
        [HttpGet]
        public IActionResult Index()
        {
            UserSignUpViewModel signUpViewModel = new UserSignUpViewModel();
            signUpViewModel.Cities = _writerCity.GetCityList();
            return View(signUpViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Index(UserSignUpViewModel signUpViewModel)
        {
            if (!signUpViewModel.IsAcceptTheContract)
            {
                ModelState.AddModelError("IsAcceptTheContract",
                    "Sayfamıza kayıt olabilmek için gizlilik sözleşmesini kabul etmeniz gerekmektedir.");
                return View(signUpViewModel);
            }
            if (ModelState.IsValid)
            {
                UserDto user = new UserDto()
                {
                    Email = signUpViewModel.Mail,
                    UserName = signUpViewModel.UserName,
                    NameSurname = signUpViewModel.NameSurname,
                    About= signUpViewModel.About,
                    City = signUpViewModel.City
                };
                await _userService.RegisterUserAsync(user, signUpViewModel.Password);
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            return View(signUpViewModel);
        }
    }
}