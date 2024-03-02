using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.Models;
using BusinessLayer.ValidationRules;
using Castle.DynamicProxy;
using Core.Extensions;
using CoreLayer.Utilities.Interceptors;
using CoreLayer.Utilities.MailUtilities;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DependencyResolvers
{
    public static class Ioc
    {
        public static void IocBusinessInstall(this IServiceCollection services)
        {
            //services ioc

            services.AddScoped<IAboutService, AboutManager>();

            services.AddScoped<IBlogService, BlogManager>();

            services.AddScoped<IBusinessUserService, UserBusinessManager>();

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
