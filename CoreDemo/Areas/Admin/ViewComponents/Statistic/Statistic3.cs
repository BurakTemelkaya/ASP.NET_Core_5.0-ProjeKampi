
using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace CoreDemo.Areas.Admin.ViewComponents.Statistic
{
    public class Statistic3 : ViewComponent
    {
        readonly IBusinessUserService _userService;
        readonly INewsLetterService _newsLetterService;
        readonly ICommentService _commentService;
        readonly ICategoryService _categoryService;
        public Statistic3(IBusinessUserService userService, INewsLetterService newsLetterService, ICommentService commentService,
            ICategoryService categoryService)
        {
            _userService = userService;
            _newsLetterService = newsLetterService;
            _commentService = commentService;
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.UserCount = await _userService.GetByUserCountAsync();
            ViewBag.LikeCommentCount = await _commentService.GetCountAsync(x => x.BlogScore > 2);
            ViewBag.NewsLetterCount = await _newsLetterService.GetCountAsync();
            ViewBag.CategoryCount = await _categoryService.GetCountAsync();
            string exchangeRate = "http://www.tcmb.gov.tr/kurlar/today.xml";
            var xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(exchangeRate);
                DateTime date = Convert.ToDateTime(xmlDoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
                ViewBag.Euro = xmlDoc.SelectSingleNode("Tarih_Date/Currency [@Kod='EUR']/BanknoteSelling").InnerXml.ToString();
                ViewBag.Dolar = xmlDoc.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/BanknoteSelling").InnerXml.ToString();
            }
            catch
            {
                ViewBag.Euro = 0;
                ViewBag.Dolar = 0;
            }
            return View();
        }
    }
}
