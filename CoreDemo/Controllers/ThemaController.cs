using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CoreDemo.Controllers
{
    public class ThemaController : Controller
    {
        [HttpPost]
        public IActionResult SetThema(string data)
        {
            CookieOptions cookies = new();
            cookies.Expires=DateTime.Now.AddYears(1);
            Response.Cookies.Append("thema", data, cookies);
            return View();
        }
    }
}
