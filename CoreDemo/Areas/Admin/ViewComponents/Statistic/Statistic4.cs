using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Areas.Admin.ViewComponents.Statistic
{
    public class Statistic4 : ViewComponent
    {
        readonly IBusinessUserService _userService;
        public Statistic4(IBusinessUserService userService)
        {
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var value = await _userService.FindByUserNameAsync(User.Identity.Name);
            ViewBag.v1 = value.NameSurname;
            ViewBag.v2 = value.ImageUrl;
            ViewBag.v3 = value.About;
            return View();
        }
    }
}
