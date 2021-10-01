using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
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

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(Writer writer,string passwordAgain)
        {
            WriterValidator wv = new WriterValidator();
            ValidationResult results = wv.Validate(writer);
            if (results.IsValid && writer.WriterPassword == passwordAgain)
            {
                writer.WriterStatus = true;
                writer.WriterAbout = "Deneme test";
                wm.WriterAdd(writer);
                return RedirectToAction("Index", "Blog");
            }
            else if(!results.IsValid)
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }                
            }
            else
            {
                ModelState.AddModelError("WriterPassword", "Girdiğiniz Şifreler Eşleşmedi Lütfen Tekrar Deneyin");
            }
            return View();
        }
        public List<SelectListItem> GetCity()
        {
            List<string> roles = new List<string>();
            roles.Add("İstanbul");
            roles.Add("İzmir");
            roles.Add("Ankara");
            List<SelectListItem> adminRole = (from x in roles
                                              select new SelectListItem
                                              {
                                                  Text = x,
                                                  Value = x
                                              }).ToList();
            return adminRole;
        }
    }
}
