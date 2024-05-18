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
        public UserDestroyerMiddleware(RequestDelegate next, IUserBusinessService businessUserService)
        {
            _next = next;
            _businessUserService = businessUserService;
        }
        /// <summary>
        /// Kullanıcı yasaklı ise sistemden çıkış yapmasını sağlayan kontrol mekanizması.
        /// </summary>
        public async Task InvokeAsync(HttpContext httpContext, SignInManager<AppUser> signInManager, string redirectUrl)
        {
            if (await BanCheck(signInManager, httpContext))
            {
                httpContext.Response.Redirect(redirectUrl);
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
