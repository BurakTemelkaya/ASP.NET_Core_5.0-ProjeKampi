using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BusinessLayer.Middleware
{
    public class UserDestroyerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUserBusinessService _businessUserService;
        private readonly string _redirectUrl;
        public UserDestroyerMiddleware(RequestDelegate next, IUserBusinessService businessUserService, string redirectUrl)
        {
            _next = next;
            _businessUserService = businessUserService;
            _redirectUrl = redirectUrl;
        }
        /// <summary>
        /// Kullanıcı yasaklı ise sistemden çıkış yapmasını sağlayan kontrol mekanizması.
        /// </summary>
        public async Task InvokeAsync(HttpContext httpContext, SignInManager<AppUser> signInManager)
        {
            if (await BanCheck(signInManager, httpContext))
            {
                httpContext.Response.Redirect(_redirectUrl);
            }
            else
            {
                await _next(httpContext);
            }           
        }

        public async Task<bool> BanCheck(SignInManager<AppUser> signInManager, HttpContext httpContext)
        {
            if (!string.IsNullOrEmpty(httpContext.User.Identity.Name))
            {
                var isBanned = await _businessUserService.GetBanDateAsync(httpContext.User.Identity.Name);
                if (isBanned.Success && isBanned.Data > System.DateTimeOffset.Now)
                {
                    await signInManager.SignOutAsync();
                    return true;
                }
            }
            return false;
        }
    }
}
