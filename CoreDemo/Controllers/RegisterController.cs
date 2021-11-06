using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using CoreDemo.Models;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    public class RegisterController : Controller
    {
        WriterManager wm = new WriterManager(new EfWriterRepository());
        WriterCity writerCity = new WriterCity();

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Cities = writerCity.GetCityList();
            return View();
        }
        [HttpPost]
        public IActionResult Index(Writer writer, string passwordAgain, IFormFile imageFile)
        {
            WriterValidator wv = new WriterValidator();
            AddProfileImage addProfileImage = new AddProfileImage();
            ValidationResult results = wv.Validate(writer);
            var validateWriter = wm.TGetByFilter(x => x.WriterMail == writer.WriterMail);
            var validateUserName = wm.TGetByFilter(x => x.WriterName == writer.WriterName);
            if (results.IsValid && writer.WriterPassword == passwordAgain && validateWriter == null)
            {
                writer.WriterStatus = true;
                writer.WriterAbout = "Deneme test";
                writer.WriterRegisterDate = DateTime.Now;
                addProfileImage.ImageAdd(imageFile, out string imageName);
                writer.WriterImage = imageName;
                wm.TAdd(writer);
                return RedirectToAction("Index", "Blog");
            }
            else if (!results.IsValid)
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            else if (writer.WriterPassword != passwordAgain)
            {
                ModelState.AddModelError("WriterPassword", "Girdiğiniz Şifreler Eşleşmedi Lütfen Tekrar Deneyin");
            }
            else if (validateWriter != null)
            {
                ModelState.AddModelError("ErrorMessage", "Girdiğiniz E-Mail Adresini Kullanan Bir Hesap Mevcut");
            }
            ViewBag.Cities = writerCity.GetCityList();//dropdown hata vermemesi için Şehir Listesini tekrar gönderdim            
            return View();
        }

    }
}
