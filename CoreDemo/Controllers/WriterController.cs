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
        WriterManager writerManager = new WriterManager(new EfWriterRepository());
        WriterCity writerCity = new WriterCity();
        [Authorize]
        public IActionResult Index()
        {
            string mail = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Email).Value.ToString();
            string id = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Name).Value;
            ViewBag.id = id;
            ViewBag.mail = mail;
            ViewBag.Name = writerManager.TGetByFilter(x => x.WriterMail == mail).WriterName;
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
            var writerValues = writerManager.TGetByFilter(x => x.WriterID == int.Parse(User.Identity.Name));
            return View(writerValues);
        }
        [HttpPost]
        public IActionResult WriterEditProfile(Writer writer, string passwordAgain, IFormFile imageFile)
        {
            WriterValidator validations = new WriterValidator();
            AddProfileImage addProfileImage = new AddProfileImage();
            var oldValues = writerManager.TGetByID(int.Parse(User.Identity.Name));
            if (writer.WriterPassword == null)
            {
                writer.WriterPassword = oldValues.WriterPassword;
                passwordAgain = writer.WriterPassword;
            }
            ValidationResult result = validations.Validate(writer);
            if (result.IsValid && writer.WriterPassword == passwordAgain)
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
                writerManager.TUpdate(writer);
                return RedirectToAction("Index", "Dashboard");
            }
            else if (!result.IsValid)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
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
