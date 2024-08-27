
using CoreLayer.CrossCuttingConcerns.Caching;
using CoreLayer.CrossCuttingConcerns.Caching.Microsoft;
using CoreLayer.Extensions;
using CoreLayer.Utilities.CaptchaUtilities;
using CoreLayer.Utilities.IoC;
using CoreLayer.Utilities.MailUtilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace CoreLayer.DependancyResolvers;

public class CoreModule : ICoreModule
{
    public void Load(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddMemoryCache();

        serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        serviceCollection.AddSingleton<ICacheManager, MemoryCacheManager>();

        MailSettings mailSettings = configuration.GetSection("MailSettings").Get<MailSettings>();

        serviceCollection.AddSingleton<IMailService, MailKitMailService>(_ => new MailKitMailService(mailSettings));

        serviceCollection.AddSingleton<ICaptchaService, RecaptchaManager>();

        serviceCollection.AddSingleton<Stopwatch>();

        serviceCollection.AddSingleton<UserHelper>();

        serviceCollection.AddSignalR();
    }
}
