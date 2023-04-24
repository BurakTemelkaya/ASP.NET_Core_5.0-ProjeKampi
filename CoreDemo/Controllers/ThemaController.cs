using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CoreDemo.Controllers
{
    [AllowAnonymous]
    public class ThemaController : Controller
    {
        [HttpPost]
        public IActionResult Set(string data)
        {
            CookieOptions cookies = new();
            cookies.Expires=DateTime.Now.AddMonths(1);
            Response.Cookies.Append("thema", data, cookies);
            return Ok();
        }
    }
}
