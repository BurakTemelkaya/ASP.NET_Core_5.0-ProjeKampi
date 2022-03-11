using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using CoreDemo.Models;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly IUserService _userService;

        public WriterController(IWriterService writerService, UserInfo userInfo, WriterCity writerCity
        , IUserService userService)
        {
            _writerService = writerService;
            _userInfo = userInfo;
            _writerCity = writerCity;
            _userService = userService;

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
            var writer = await _userService.GetByUserNameAsync(User.Identity.Name);
            return View(writer);
        }
        [HttpPost]
        public IActionResult WriterEditProfile(Writer writer, string passwordAgain, IFormFile imageFile)
        {
            var oldValues = _writerService.TGetByID(_userInfo.GetID(User));
            if (writer.WriterPassword == null)
            {
                writer.WriterPassword = oldValues.WriterPassword;
                passwordAgain = writer.WriterPassword;
            }
            if (ModelState.IsValid && writer.WriterPassword == passwordAgain)
            {
                if (imageFile == null)
                {
                    writer.WriterImage = oldValues.WriterImage;
                }
                else
                {
                    writer.WriterImage = AddProfileImage.ImageAdd(imageFile);
                }
                //eski bilgilerin silinmemesi için tekrar eski verileri kaydetme
                writer.WriterStatus = oldValues.WriterStatus;
                writer.WriterRegisterDate = oldValues.WriterRegisterDate;
                _writerService.TUpdate(writer);
                return RedirectToAction("Index", "Dashboard");
            }
            else if (writer.WriterPassword != passwordAgain && writer.WriterPassword != oldValues.WriterPassword)
            {
                ModelState.AddModelError("PasswordAgainMessage",
                    "Girdiğiniz Şifreler Eşleşmedi Lütfen Tekrar Deneyin");
            }
            ViewBag.Cities = _writerCity.GetCityList();
            return View();
        }
    }
}
