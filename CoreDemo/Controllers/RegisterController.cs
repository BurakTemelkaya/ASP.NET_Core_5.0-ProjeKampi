using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using CoreDemo.Models;
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
            ViewBag.Cities = GetCityList();
            return View();
        }
        [HttpPost]
        public IActionResult Index(Writer writer, string passwordAgain)
        {
            WriterValidator wv = new WriterValidator();
            ValidationResult results = wv.Validate(writer);
            var validateWriter = wm.GetWriterByMail(writer.WriterMail);
            if (results.IsValid && writer.WriterPassword == passwordAgain && validateWriter == null)
            {
                writer.WriterStatus = true;
                writer.WriterAbout = "Deneme test";
                writer.WriterRegisterDate = DateTime.Now;
                wm.WriterAdd(writer);
                //writersAndCities.City;
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
            ViewBag.Cities = GetCityList();//dropdown hata vermemesi için Şehir Listesini tekrar gönderdim            
            return View();
        }
        public List<SelectListItem> GetCityList()
        {
            String[] CitiesArray = { "Adana", "Adıyaman", "Afyon", "Ağrı", "Aksaray", "Amasya", "Ankara", "Antalya", "Ardahan", "Artvin", "Aydın", "Bartın", "Batman", "Balıkesir", "Bayburt", "Bilecik", "Bingöl", "Bitlis", "Bolu", "Burdur", "Bursa", "Çanakkale", "Çankırı", "Çorum", "Denizli", "Diyarbakır", "Düzce", "Edirne", "Elazığ", "Erzincan", "Erzurum", "Eskişehir", "Gaziantep", "Giresun", "Gümüşhane", "Hakkari", "Hatay", "Iğdır", "Isparta", "İçel", "İstanbul", "İzmir", "Karabük", "Karaman", "Kars", "Kastamonu", "Kayseri", "Kırıkkale", "Kırklareli", "Kırşehir", "Kilis", "Kocaeli", "Konya", "Kütahya", "Malatya", "Manisa", "Kahramanmaraş", "Mardin", "Muğla", "Muş", "Nevşehir", "Niğde", "Ordu", "Osmaniye", "Rize", "Sakarya", "Samsun", "Siirt", "Sinop", "Sivas", "Tekirdağ", "Tokat", "Trabzon", "Tunceli", "Şanlıurfa", "Şırnak", "Uşak", "Van", "Yalova", "Yozgat", "Zonguldak" };
            new List<string>(CitiesArray);//arrayi liste çevirme
            //şehirleri list şeklinde bulamadığımdan array şeklinde alıp bunu listeye çevirdim
            List<SelectListItem> cities = (from x in CitiesArray
                                           select new SelectListItem
                                           {
                                               Text = x,
                                               Value = x
                                           }).ToList();
            return cities;
        }
    }
}
