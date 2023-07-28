using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Middleware
{
    public class UserDestroyerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IBusinessUserService _businessUserService;
        private DateTime ControlTime = DateTime.Now.AddMinutes(1);
        public UserDestroyerMiddleware(RequestDelegate next, IBusinessUserService businessUserService)
        {
            _next = next;
            _businessUserService = businessUserService;
        }
        /// <summary>
        /// Kullanıcı yasaklı ise sistemden çıkış yapmasını sağlayan kontrol mekanizması.
        /// </summary>
        public async Task Invoke(HttpContext httpContext, SignInManager<AppUser> signInManager)
        {
            if (!string.IsNullOrEmpty(httpContext.User.Identity.Name))
            {
                if (ControlTime > DateTime.Now)
                {
                    await _next(httpContext);
                    return;
                }
                var user = await _businessUserService.GetByUserNameAsync(httpContext.User.Identity.Name);
                if (user.Data.LockoutEnd > DateTime.Now)
                {
                    await signInManager.SignOutAsync();
                    httpContext.Response.Redirect("/Blog/Index");
                }
            }
            ControlTime = DateTime.Now.AddMinutes(1);
            await _next(httpContext);
        }
    }
}
