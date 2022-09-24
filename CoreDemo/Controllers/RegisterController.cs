using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using CoreDemo.Models;
using DataAccessLayer.EntityFramework;
using EntityLayer.DTO;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        private readonly IBusinessUserService _userService;

        private readonly WriterCity _writerCity;

        public RegisterController(IBusinessUserService userService, WriterCity writerCity)
        {
            _userService = userService;
            _writerCity = writerCity;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Cities = _writerCity.GetCityList();
            return View();
        }
        /// <summary>
        /// kullanıcının bilgilerinin önceden kullanılmadığını ve doğru olduğunu kontrol edip kullanıcıyı kayıt etme işlemi.
        /// </summary>
        /// <param name="user"></param>
        /// kullanıcının bilgilerinin yer aldığı nesne
        /// <param name="passwordAgain"></param>
        /// parola kontrolü için kullanılan nesne
        /// <param name="imageFile"></param>
        /// kullanıcının resmini kaydetmek için kullanılan nesne
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Index(UserDto user, string passwordAgain, IFormFile imageFile)
        {
            var validateUserName = await _userService.FindByUserNameAsync(user.UserName);
            var validateEmail = await _userService.FindByMailAsync(user.Email);
            if (ModelState.IsValid && user.Password == passwordAgain && validateUserName == null && validateEmail == null)
            {
                user.ImageUrl = AddProfileImage.ImageAdd(imageFile);
                await _userService.RegisterUserAsync(user, user.Password);
                return RedirectToAction("Index", "Blog");
            }
            else if (user.Password != passwordAgain)
            {
                ModelState.AddModelError("WriterPassword", "Girdiğiniz şifreler eşleşmedi lütfen tekrar deneyin");
            }
            else if (validateEmail != null)
            {
                ModelState.AddModelError("ErrorMessage", "Girdiğiniz e-maili kullanan bir hesap mevcut." +
                    "Lütfen başka bir Email ile kayıt yapmayı deneyin.");
            }
            else if (validateUserName != null)
            {
                ModelState.AddModelError("ErrorMessage", "Girdiğiniz kullanıcı adını kullanan bir hesap mevcut." +
                    " Lütfen başka bir kullanıcı adı ile kayıt olmayı yapmayı deneyin.");
            }
            ViewBag.Cities = _writerCity.GetCityList();
            return View(user);
        }

    }
}
