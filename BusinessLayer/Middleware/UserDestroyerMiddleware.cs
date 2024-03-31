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
        private readonly IHttpContextAccessor _contextAccessor;
        public UserDestroyerMiddleware(RequestDelegate next, IUserBusinessService businessUserService, IHttpContextAccessor httpContextAccessor)
        {
            _next = next;
            _businessUserService = businessUserService;
            _contextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// Kullanıcı yasaklı ise sistemden çıkış yapmasını sağlayan kontrol mekanizması.
        /// </summary>
        public async Task Invoke(HttpContext httpContext, SignInManager<AppUser> signInManager)
        {
            if (!string.IsNullOrEmpty(httpContext.User.Identity.Name))
            {
                var isBanned = await _businessUserService.GetBanDateAsync(_contextAccessor.HttpContext.User.Identity.Name);
                if (isBanned.Success && isBanned.Data > System.DateTimeOffset.Now)
                {
                    await signInManager.SignOutAsync();
                    httpContext.Response.Redirect("/Blog/Index");
                }
            }
            await _next(httpContext);
        }
    }
}
