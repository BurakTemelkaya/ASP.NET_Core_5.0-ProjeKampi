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

        public UserDestroyerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext httpContext,
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager)
        {
            if (!string.IsNullOrEmpty(httpContext.User.Identity.Name))
            {
                var user = await userManager.FindByNameAsync(httpContext.User.Identity.Name);
                if (user.LockoutEnd > DateTimeOffset.Now)
                {
                    await signInManager.SignOutAsync();
                    httpContext.Response.Redirect("/Blog/Index");
                }
            }
            await _next(httpContext);
        }
    }
}
