using Autofac;
using Autofac.Extras.DynamicProxy;
using BusinessLayer.Models;
using BusinessLayer.ValidationRules;
using Castle.DynamicProxy;
using CoreLayer.Utilities.Interceptors;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DependencyResolvers
{
    public class AutofacValidationModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BlogValidator>().As<IValidator<Blog>>().SingleInstance();

            builder.RegisterType<CategoryValidator>().As<IValidator<Category>>().SingleInstance();

            builder.RegisterType<CommentValidator>().As<IValidator<Comment>>().SingleInstance();

            builder.RegisterType<ContactValidator>().As<IValidator<Contact>>().SingleInstance();

            builder.RegisterType<MessageValidator>().As<IValidator<Message>>().SingleInstance();

            builder.RegisterType<NewsLetterSendMailsModelValidator>().As<IValidator<NewsLetterSendMailsModel>>().SingleInstance();

            builder.RegisterType<NewsLetterValidator>().As<IValidator<NewsLetter>>().SingleInstance();

            builder.RegisterType<ResetPasswordDtoValidator>().As<IValidator<ResetPasswordDto>>().SingleInstance();

            builder.RegisterType<UserSignUpDtoValidator>().As<IValidator<UserSignUpDto>>().SingleInstance();

            builder.RegisterType<UserValidator>().As<IValidator<UserDto>>().SingleInstance();
        }
        
    }
}
