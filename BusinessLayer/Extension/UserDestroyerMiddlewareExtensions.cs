using BusinessLayer.Middleware;
using Microsoft.AspNetCore.Builder;

namespace BusinessLayer.Extension
{
    public static class UserDestroyerMiddlewareExtensions
    {
        public static IApplicationBuilder UseUserDestroyer(this IApplicationBuilder builder, string redirectUrl)
        {
            return builder.UseMiddleware<UserDestroyerMiddleware>(redirectUrl);
        }
    }
}
