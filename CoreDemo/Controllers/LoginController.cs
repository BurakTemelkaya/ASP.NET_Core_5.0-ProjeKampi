using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using CoreDemo.Models;
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

        public LoginController(SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(UserSignInViewModel appUser)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(appUser.UserName, appUser.Password, appUser.IsPersistent, true);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    TempData["ErrorMessage"] = "Kullanıcı adınız veya parolanız hatalı lütfen tekrar deneyiniz.";
                    return View(appUser);
                }
            }
            return View(appUser);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
