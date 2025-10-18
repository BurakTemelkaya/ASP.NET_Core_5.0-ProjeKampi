
using CoreLayer.BackgroundTasks;
using CoreLayer.CrossCuttingConcerns.Caching;
using CoreLayer.CrossCuttingConcerns.Caching.Microsoft;
using CoreLayer.Extensions;
using CoreLayer.Utilities.CaptchaUtilities;
using CoreLayer.Utilities.IoC;
using CoreLayer.Utilities.MailUtilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace CoreLayer.DependancyResolvers;

public class CoreModule : ICoreModule
{
    public void Load(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddMemoryCache();

        serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        serviceCollection.AddSingleton<ICacheManager, MemoryCacheManager>();

        serviceCollection.AddSingleton<IMailService, MailKitMailService>(p => new MailKitMailService(p.GetRequiredService<IOptions<MailSettings>>().Value));

        serviceCollection.AddSingleton<ICaptchaService, RecaptchaManager>();

        serviceCollection.AddSingleton<Stopwatch>();

        serviceCollection.AddSingleton<UserHelper>();

        serviceCollection.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
        serviceCollection.AddHostedService<QueuedHostedService>();
    }
}