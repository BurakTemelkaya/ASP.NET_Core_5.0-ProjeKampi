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

        public WriterController(IWriterService writerService, UserInfo userInfo)
        {
            _writerService = writerService;
            _userInfo = userInfo;
        }
        WriterCity writerCity = new WriterCity();

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
        public IActionResult WriterEditProfile()
        {
            ViewBag.Cities = writerCity.GetCityList();
            var writerValues = _writerService.TGetByFilter(x => x.WriterID == _userInfo.GetID(User));
            return View(writerValues);
        }
        [HttpPost]
        public IActionResult WriterEditProfile(Writer writer, string passwordAgain, IFormFile imageFile)
        {
            AddProfileImage addProfileImage = new AddProfileImage();
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
                    addProfileImage.ImageAdd(imageFile, out string fileName);
                    writer.WriterImage = fileName;
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
            ViewBag.Cities = writerCity.GetCityList();
            return View();
        }      
    }
}
