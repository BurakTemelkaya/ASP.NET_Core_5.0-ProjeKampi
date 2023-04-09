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

            services.AddSingleton<IAboutService, AboutManager>();

            services.AddScoped<IBlogService, BlogManager>();

            services.AddScoped<IBusinessUserService, UserBusinessManager>();

            services.AddSingleton<ICategoryService, CategoryManager>();

            services.AddScoped<ICommentService, CommentManager>();

            services.AddScoped<IContactService, ContactManager>();

            services.AddScoped<IMessageService, MessageManager>();

            services.AddScoped<INewsLetterService, NewsLetterManager>();

            services.AddScoped<INewsLetterDraftService, NewsLetterDraftManager>();

            services.AddScoped<INotificationService, NotificationManager>();

            services.AddScoped<IContactService, ContactManager>();

            services.AddScoped<IMessageDraftService, MessageDraftManager>();


            //validators ioc

            //services.AddSingleton<IValidator<Blog>, BlogValidator>();

            //services.AddSingleton<IValidator<Category>, CategoryValidator>();

            //services.AddSingleton<IValidator<Comment>, CommentValidator>();

            //services.AddSingleton<IValidator<Contact>, ContactValidator>();

            //services.AddSingleton<IValidator<UserDto>, UserValidator>();

            //services.AddSingleton<IValidator<NewsLetter>, NewsLetterValidator>();

            //services.AddSingleton<IValidator<NewsLetterSendMailsModel>, NewsLetterSendMailsModelValidator>();

            //services.AddSingleton<IValidator<UserSignUpDto>, UserSignUpDtoValidator>();

            //services.AddSingleton<IValidator<ResetPasswordDto>, ResetPasswordDtoValidator>();

            //services.AddSingleton<IValidator<Message>, MessageValidator>();
        }
    }
}
