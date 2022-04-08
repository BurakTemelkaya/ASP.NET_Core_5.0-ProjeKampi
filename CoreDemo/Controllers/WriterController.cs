using AutoMapper;
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using CoreDemo.Models;
using DataAccessLayer.EntityFramework;
using EntityLayer;
using EntityLayer.Concrete;
using FluentValidation.Results;
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
        private readonly IWriterService _writerService;
        private readonly UserInfo _userInfo;
        private readonly WriterCity _writerCity;
        private readonly IBusinessUserService _userManager;

        public WriterController(IWriterService writerService, UserInfo userInfo, WriterCity writerCity
        , IBusinessUserService userManager)
        {
            _writerService = writerService;
            _userInfo = userInfo;
            _writerCity = writerCity;
            _userManager = userManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            string mail = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Email).Value.ToString();
            string id = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Name).Value;
            ViewBag.id = id;
            ViewBag.mail = mail;
            ViewBag.Name = _writerService.TGetByFilter(x => x.WriterMail == mail).WriterName;
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
            ViewBag.Cities = _writerCity.GetCityList();
            var writer = await _userManager.FindUserNameAsync(User.Identity.Name);
            return View(writer);
        }
        [HttpPost]
        public async Task<IActionResult> WriterEditProfile(UserDto userDto, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    userDto.ImageUrl = AddProfileImage.ImageAdd(imageFile);
                }
                await _userManager.UpdateUserAsync(userDto);
                return RedirectToAction("Index", "Dashboard");
            }
            ViewBag.Cities = _writerCity.GetCityList();
            return View();
        }
    }
}
