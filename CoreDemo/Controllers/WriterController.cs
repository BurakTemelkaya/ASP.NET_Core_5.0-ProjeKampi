using AutoMapper;
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using CoreDemo.Models;
using DataAccessLayer.EntityFramework;
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
        private readonly UserInfo _userInfo;
        private readonly WriterCity _writerCity;
        private readonly IBusinessUserService _userManager;

        public WriterController(UserInfo userInfo, WriterCity writerCity
        , IBusinessUserService userManager)
        {
            _userInfo = userInfo;
            _writerCity = writerCity;
            _userManager = userManager;
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
        public IActionResult WriterProfile()
        {
            return View();
        }
        public IActionResult WriterMail()
        {
            return View();
        }
        public IActionResult Test()
        {
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
            ViewBag.Cities = _writerCity.GetCityListAsync();
            var writer = await _userManager.FindByUserNameAsync(User.Identity.Name);
            if (writer.ImageUrl[..5] != "https" || writer.ImageUrl[..4] != "http")
                writer.ImageUrl = null;
            return View(writer);
        }
        [HttpPost]
        public async Task<IActionResult> WriterEditProfile(UserDto userDto, IFormFile imageFile)
        {
            if (userDto.UserName == null || userDto.NameSurname == null || userDto.Email == null ||
                userDto.City == null || userDto.About == null)
            {
                ModelState.AddModelError("UserName", "Lütfen profil bilgilerinizi boş bırakmayın.");
                ViewBag.Cities = _writerCity.GetCityListAsync();
                return View(userDto);
            }
            if (imageFile != null)
            {
                userDto.ImageUrl = AddImage.ImageAdd(imageFile, AddImage.StaticProfileImageLocation());
            }
            await _userManager.UpdateUserAsync(userDto);
            return RedirectToAction("Index", "Dashboard");
        }
    }
}
