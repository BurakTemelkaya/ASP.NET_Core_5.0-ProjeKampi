using CoreLayer.CrossCuttingConcerns.Caching.Microsoft;
using CoreLayer.CrossCuttingConcerns.Caching;
using CoreLayer.Utilities.CaptchaUtilities;
using CoreLayer.Utilities.MailUtilities;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.DependancyResolvers
{
    public static class Ioc
    {
        public static void IocCoreInstall(this IServiceCollection services)
        {
            services.AddTransient<IMailService, OutlookMailManager>();

            services.AddTransient<ICaptchaService, RecaptchaManager>();

            services.AddTransient<ICaptchaService, RecaptchaManager>();

            services.AddMemoryCache();

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<ICacheManager, MemoryCacheManager>();
        }
    }
}
