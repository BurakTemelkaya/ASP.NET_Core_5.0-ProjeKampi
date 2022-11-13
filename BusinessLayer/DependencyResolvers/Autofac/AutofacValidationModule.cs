using Autofac;
using BusinessLayer.ValidationRules;
using EntityLayer.DTO;
using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Models;

namespace BusinessLayer.DependencyResolvers.Autofac
{
    public class AutofacValidationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BlogValidator>().As<IValidator<Blog>>().SingleInstance();

            builder.RegisterType<CategoryValidator>().As<IValidator<Category>>().SingleInstance();

            builder.RegisterType<ContactValidator>().As<IValidator<Contact>>().SingleInstance();

            builder.RegisterType<UserValidator>().As<IValidator<UserDto>>().SingleInstance();

            builder.RegisterType<NewsLetterValidator>().As<IValidator<NewsLetter>>().SingleInstance();

            builder.RegisterType<NewsLetterSendMailsModelValidator>().As<IValidator<NewsLetterSendMailsModel>>().SingleInstance();

            builder.RegisterType<UserSignUpDtoValidator>().As<IValidator<UserSignUpDto>>().SingleInstance();

            builder.RegisterType<ResetPasswordDtoValidator>().As<IValidator<ResetPasswordDto>>().SingleInstance();

            builder.RegisterType<MessageValidator>().As<IValidator<Message>>().SingleInstance();
        }
    }
}
