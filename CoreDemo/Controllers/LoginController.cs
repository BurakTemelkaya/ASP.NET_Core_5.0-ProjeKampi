using BusinessLayer.Abstract;
using CoreDemo.Models;
using CoreLayer.Utilities.CaptchaUtilities;
using DocumentFormat.OpenXml.Spreadsheet;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUserBusinessService _userService;
        private readonly ICaptchaService _captchaService;
        private readonly ILoginLoggerService _loginLoggerService;

        public LoginController(SignInManager<AppUser> signInManager, IUserBusinessService userService,
            ICaptchaService captchaService, ILoginLoggerService loginLoggerService)
        {
            _signInManager = signInManager;
            _userService = userService;
            _captchaService = captchaService;
            _loginLoggerService = loginLoggerService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity.Name != null)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            ViewBag.SiteKey = _captchaService.GetSiteKey();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(UserSignInViewModel appUser, string returnUrl)
        {
            string captchaMessage = await _captchaService.RecaptchaControl();
            if (!string.IsNullOrEmpty(captchaMessage))
            {
                ModelState.AddModelError("Captcha", captchaMessage);
                ViewBag.SiteKey = _captchaService.GetSiteKey();
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(appUser.UserName, appUser.Password, appUser.IsPersistent, true);
            if (result.Succeeded)
            {
                await _loginLoggerService.AddAsync(appUser.UserName);
                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                return RedirectToAction("Index", "Dashboard");
            }
            else if (result.IsNotAllowed)
            {
                TempData["ErrorMessage"] = "Giriş yapabilmek için mail adresinize gelen linke tıklayarak doğrulamanızı yapınız." +
                    " Eğer mail gelmediyse tekrar doğrulama maili gönderme işlemini aşağıdaki adresten yapabilirsiniz.";
                ViewBag.Url = Url.ActionLink("ReSendConfirmationMail", "Register", Request.Scheme);
            }
            else
            {
                if (result.IsLockedOut)
                {
                    var user = await _userService.GetByUserNameAsync(appUser.UserName);
                    TempData["ErrorMessage"] = "Hesabınız " + Convert.ToDateTime(user.Data.LockoutEnd.ToString()) + " tarihine kadar yasaklanmıştır.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Kullanıcı adınız veya parolanız hatalı lütfen tekrar deneyiniz.";
                }            
            }
            ViewBag.SiteKey = _captchaService.GetSiteKey();
            return View(appUser);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Blog");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
