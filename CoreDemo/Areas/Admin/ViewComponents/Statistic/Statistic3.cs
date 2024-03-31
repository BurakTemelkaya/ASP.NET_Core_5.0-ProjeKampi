
using BusinessLayer.Abstract;
using BusinessLayer.Constants;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace CoreDemo.Areas.Admin.ViewComponents.Statistic
{
    public class Statistic3 : ViewComponent
    {
        private readonly IUserBusinessService _userService;
        private readonly INewsLetterService _newsLetterService;
        private readonly ICommentService _commentService;
        private readonly ICategoryService _categoryService;
        private readonly ICurrencyService _currencyService;
        public Statistic3(IUserBusinessService userService, INewsLetterService newsLetterService, ICommentService commentService,
            ICategoryService categoryService, ICurrencyService currencyService)
        {
            _userService = userService;
            _newsLetterService = newsLetterService;
            _commentService = commentService;
            _categoryService = categoryService;
            _currencyService = currencyService;
        }

        /// <summary>
        /// Dövizlerin verisini tek tek çekmektense hepsini bi anda çekmemin sebebi sitenin ard arda istek atınca çalışmaması
        /// </summary>
        /// <returns></returns>
        public IViewComponentResult Invoke()
        {
            ViewBag.UserCount = _userService.GetByUserCountAsync().Result.Data;
            ViewBag.LikeCommentCount = _commentService.GetCountAsync(x => x.BlogScore > 2).Result.Data;
            ViewBag.NewsLetterCount = _newsLetterService.GetCountAsync().Result.Data;
            ViewBag.CategoryCount = _categoryService.GetCountAsync().Result.Data;


            var currencies = _currencyService.GetCurrencys(CurrencyCodes.Euro, CurrencyCodes.Dollar);
            if (currencies.Success)
            {
                ViewBag.Euro = currencies.Data.First(x => x.Code == CurrencyCodes.Euro).Value;
                ViewBag.Dolar = currencies.Data.First(x => x.Code == CurrencyCodes.Dollar).Value;
            }
            else
            {
                ViewBag.Euro = currencies.Message;
                ViewBag.Dolar = currencies.Message;
            }

            return View();
        }
    }
}
