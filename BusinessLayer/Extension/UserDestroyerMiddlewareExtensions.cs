using BusinessLayer.Middleware;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Extension
{
    public static class UserDestroyerMiddlewareExtensions
    {
        public static IApplicationBuilder UseUserDestroyer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserDestroyerMiddleware>();
        }
    }
}
