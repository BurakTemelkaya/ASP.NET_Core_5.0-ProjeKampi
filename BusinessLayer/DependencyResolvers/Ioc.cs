using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.Models;
using BusinessLayer.ValidationRules;
using Core.Extensions;
using CoreLayer.Utilities.MailUtilities;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DependencyResolvers
{
    public static class Ioc
    {
        public static void IocBusinessInstall(this IServiceCollection services)
        {
            //services ioc

            services.AddTransient<IAboutService, AboutManager>();

            services.AddTransient<IBlogService, BlogManager>();

            services.AddTransient<IBusinessUserService, UserBusinessManager>();

            services.AddTransient<ICategoryService, CategoryManager>();

            services.AddTransient<ICommentService, CommentManager>();

            services.AddTransient<IContactService, ContactManager>();

            services.AddTransient<IMessageService, MessageManager>();

            services.AddTransient<INewsLetterService, NewsLetterManager>();

            services.AddTransient<INewsLetterDraftService, NewsLetterDraftManager>();

            services.AddTransient<INotificationService, NotificationManager>();

            services.AddTransient<IContactService, ContactManager>();

            services.AddTransient<IMessageDraftService, MessageDraftManager>();
            

            //validators ioc

            services.AddSingleton<IValidator<Blog>, BlogValidator>();

            services.AddSingleton<IValidator<Category>, CategoryValidator>();

            services.AddSingleton<IValidator<Comment>, CommentValidator>();

            services.AddSingleton<IValidator<Contact>, ContactValidator>();

            services.AddSingleton<IValidator<UserDto>, UserValidator>();

            services.AddSingleton<IValidator<NewsLetter>, NewsLetterValidator>();

            services.AddSingleton<IValidator<NewsLetterSendMailsModel>, NewsLetterSendMailsModelValidator>();

            services.AddSingleton<IValidator<UserSignUpDto>, UserSignUpDtoValidator>();

            services.AddSingleton<IValidator<ResetPasswordDto>, ResetPasswordDtoValidator>();

            services.AddSingleton<IValidator<Message>, MessageValidator>();
        }
    }
}
