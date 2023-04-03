using Autofac;
using Autofac.Core;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using CoreLayer.CrossCuttingConcerns.Caching.Microsoft;
using CoreLayer.CrossCuttingConcerns.Caching;
using CoreLayer.Utilities.CaptchaUtilities;
using CoreLayer.Utilities.Interceptors;
using CoreLayer.Utilities.MailUtilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLayer.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace CoreLayer.DependancyResolvers
{
    public class CoreModule: ICoreModule
    {
        public void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMemoryCache();

            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            serviceCollection.AddSingleton<ICacheManager, MemoryCacheManager>();

            serviceCollection.AddSingleton<IMailService, OutlookMailManager>();

            serviceCollection.AddSingleton<ICaptchaService, RecaptchaManager>();

            serviceCollection.AddSingleton<Stopwatch>();
        }
    }
}
