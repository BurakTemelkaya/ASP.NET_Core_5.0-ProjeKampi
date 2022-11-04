using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using CoreDemo.Models;
using CoreLayer.Utilities.CaptchaUtilities;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IBusinessUserService _userService;
        private readonly ICaptchaService _captchaService;

        public LoginController(SignInManager<AppUser> signInManager, IBusinessUserService userService, ICaptchaService captchaService)
        {
            _signInManager = signInManager;
            _userService = userService;
            _captchaService = captchaService;
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
            string captchaMessage = await _captchaService.RecaptchaControl(HttpContext);
            if (!string.IsNullOrEmpty(captchaMessage))
            {
                ModelState.AddModelError("Captcha", captchaMessage);
                ViewBag.SiteKey = _captchaService.GetSiteKey();
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(appUser.UserName, appUser.Password, appUser.IsPersistent, true);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                if (result.IsLockedOut)
                {
                    var user = await _userService.FindByUserNameAsync(appUser.UserName);
                    TempData["ErrorMessage"] = "Hesabınız " + Convert.ToDateTime(user.LockoutEnd.ToString()).ToLocalTime() + " tarihine kadar yasaklanmıştır.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Kullanıcı adınız veya parolanız hatalı lütfen tekrar deneyiniz.";
                }
                ViewBag.SiteKey = _captchaService.GetSiteKey();
                return View(appUser);
            }
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
