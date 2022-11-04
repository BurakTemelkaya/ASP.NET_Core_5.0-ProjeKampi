using CoreLayer.CrossCuttingConcerns.Caching;
using CoreLayer.CrossCuttingConcerns.Caching.Microsoft;
using CoreLayer.Utilities.CaptchaUtilities;
using CoreLayer.Utilities.IoC;
using CoreLayer.Utilities.MailUtilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.DependancyResolvers
{
    public class CoreModule : ICoreModule
    {
        public void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMemoryCache();
            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            serviceCollection.AddSingleton<ICacheManager, MemoryCacheManager>();
            serviceCollection.AddSingleton<IMailService, OutlookMailManager>();
            serviceCollection.AddSingleton<ICaptchaService, RecaptchaManager>();
        }
    }
}
