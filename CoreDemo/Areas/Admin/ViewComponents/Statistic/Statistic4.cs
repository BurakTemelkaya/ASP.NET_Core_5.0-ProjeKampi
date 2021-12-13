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
        private readonly IAdminService _adminService;

        public Statistic4(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public IViewComponentResult Invoke()
        {
            var value = _adminService.TGetByFilter(x => x.AdminID == 1);
            ViewBag.v1 = value.Name;
            ViewBag.v2 = value.ImageURL;
            ViewBag.v3 = value.ShortDescription;
            return View();
        }
    }
}
