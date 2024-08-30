using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.Concrete.EntityFramework;
using DataAccessLayer.Concrete.Stores;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DataAccessLayer.DependancyInjection
{
    public static class Ioc
    {
        public static void IocDataAccessInstall(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Context>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SQLServer")), ServiceLifetime.Scoped);

            services.AddScoped<ICategoryDal, EfCategoryRepository>();

            services.AddScoped<IAboutDal, EfAboutRepository>();

            services.AddScoped<IBlogDal, EfBlogRepository>();

            services.AddScoped<IBlogViewDal, EfBlogViewRepository>();

            services.AddScoped<ICommentDal, EfCommentRepository>();

            services.AddScoped<IContactDal, EfContactRepository>();

            services.AddScoped<IMessageDal, EfMessageRepository>();

            services.AddScoped<INewsLetterDal, EfNewsLetterRepository>();

            services.AddScoped<INotificationDal, EfNotificationRepository>();

            services.AddScoped<IMessageDraftDal, EfMessageDraftRepository>();

            services.AddScoped<INewsLetterDraftDal, EfNewsLetterDraftRepository>();

            services.AddScoped<ILogDal, EfLogRepository>();

            services.AddScoped<ILoginLoggerDal, EfLoginLoggerRepository>();

            services.AddScoped<ITicketStore, TicketStore>();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.SlidingExpiration = true;
                options.SessionStore = new TicketStore(new Context(
                    services.BuildServiceProvider().GetRequiredService<DbContextOptions<Context>>()));
                options.LoginPath = "/Login/Index";
                options.LogoutPath = "/Login/Logout";
                options.AccessDeniedPath = new PathString("/Login/AccessDenied");
            });
        }
    }
}
