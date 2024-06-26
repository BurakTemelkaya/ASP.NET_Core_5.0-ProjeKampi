﻿using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoreDemo.Areas.Admin.ViewComponents.Navbar
{
    [Authorize(Roles = "Admin,Moderator")]
    public class NavbarProfile : ViewComponent
    {
        readonly IUserBusinessService _businessUserService;
        public NavbarProfile(IUserBusinessService businessUserService)
        {
            _businessUserService = businessUserService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _businessUserService.GetByUserNameAsync(User.Identity.Name);
            var roles = await _businessUserService.GetUserRoleListAsync(user.Data);

            if (roles.Success)
            {
                string role = "";
                foreach (var item in roles.Data)
                    role += item + ",";
                role = role[..^1];
                ViewBag.Role = role;
            }

            return View(user.Data);
        }
    }
}
