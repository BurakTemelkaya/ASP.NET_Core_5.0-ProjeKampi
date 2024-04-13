using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLayer.DependencyResolvers
{
    public static class Ioc
    {
        public static void IocBusinessInstall(this IServiceCollection services)
        {
            //services ioc

            services.AddScoped<IAboutService, AboutManager>();

            services.AddScoped<IBlogService, BlogManager>();

            services.AddScoped<IUserBusinessService, UserBusinessManager>();

            services.AddScoped<ICategoryService, CategoryManager>();

            services.AddScoped<ICommentService, CommentManager>();

            services.AddScoped<IContactService, ContactManager>();

            services.AddScoped<IMessageService, MessageManager>();

            services.AddScoped<INewsLetterService, NewsLetterManager>();

            services.AddScoped<INewsLetterDraftService, NewsLetterDraftManager>();

            services.AddScoped<INotificationService, NotificationManager>();

            services.AddScoped<IContactService, ContactManager>();

            services.AddScoped<IMessageDraftService, MessageDraftManager>();

            services.AddScoped<ILogService, LogManager>();

            services.AddSingleton<ICurrencyService, CurrencyManager>();

            services.AddScoped<ILoginLoggerService, LoginLoggerManager>();
        }
    }
}
