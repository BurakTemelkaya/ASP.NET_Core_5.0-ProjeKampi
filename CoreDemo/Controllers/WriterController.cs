using AutoMapper;
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using CoreDemo.Models;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using EntityLayer.DTO;
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
    public class WriterController : Controller
    {
        private readonly WriterCity _writerCity;
        private readonly IBusinessUserService _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        protected IMapper _mapper { get; }

        public WriterController(WriterCity writerCity
        , IBusinessUserService userManager, SignInManager<AppUser> signInManager, IMapper mapper)
        {
            _writerCity = writerCity;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            string mail = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Email).Value.ToString();
            string id = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Name).Value;
            ViewBag.id = id;
            ViewBag.mail = mail;
            ViewBag.Name = await _userManager.FindByMailAsync(mail);
            return View();
        }
        [AllowAnonymous]
        public PartialViewResult WriterNavbarPartial()
        {
            return PartialView();
        }
        [AllowAnonymous]
        public PartialViewResult WriterFooterPartial()
        {
            return PartialView();
        }
        [HttpGet]
        public async Task<IActionResult> WriterEditProfile()
        {
            ViewBag.Cities = await _writerCity.GetCityListAsync();
            var writer = await _userManager.FindByUserNameAsync(User.Identity.Name);
            if (writer.ImageUrl != null && writer.ImageUrl[..5] != "https" || writer.ImageUrl[..4] != "http")
                writer.ImageUrl = null;
            return View(writer);
        }
        [HttpPost]
        public async Task<IActionResult> WriterEditProfile(UserDto userDto)
        {
            var oldValue = await _userManager.GetByIDAsync(userDto.Id.ToString());
            var result = await _userManager.UpdateUserAsync(userDto);
            if (result != null)
            {
                ModelState.AddModelError("Email", "Kullanıcı bilgilerinizi güncellerken bir hata meydana geldi." +
                    " Lütfen daha sonra tekrar deneyiniz");
                ViewBag.Cities = await _writerCity.GetCityListAsync();
                return View(userDto);
            }
            var user = await _userManager.GetByIDAsync(userDto.Id.ToString());
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(user, isPersistent: true);
            if (user.PasswordHash == oldValue.PasswordHash && userDto.Password != null &&
                userDto.PasswordAgain != null && userDto.OldPassword != null)
            {
                ModelState.AddModelError("Password", "Parola güncellenirken bir hata oluştu lütfen değerleri düzgün girdiğinizden" +
                    "emin olunuz. Eğer Düzenlediyseniz diğer bilgileriniz güncellenmiştir.");
            }
            ViewBag.Cities = await _writerCity.GetCityListAsync();
            return View(userDto);
        }
    }
}
