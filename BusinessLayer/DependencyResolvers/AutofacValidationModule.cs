using Autofac;
using BusinessLayer.Models;
using BusinessLayer.ValidationRules;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using FluentValidation;

namespace BusinessLayer.DependencyResolvers
{
    public class AutofacValidationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AboutValidator>().As<IValidator<About>>().SingleInstance();

            builder.RegisterType<BlogValidator>().As<IValidator<Blog>>().SingleInstance();

            builder.RegisterType<GetBlogModelValidator>().As<IValidator<GetBlogModel>>().SingleInstance();

            builder.RegisterType<CategoryValidator>().As<IValidator<Category>>().SingleInstance();

            builder.RegisterType<CommentValidator>().As<IValidator<Comment>>().SingleInstance();

            builder.RegisterType<ContactValidator>().As<IValidator<Contact>>().SingleInstance();

            builder.RegisterType<MessageValidator>().As<IValidator<Message>>().SingleInstance();

            builder.RegisterType<NewsLetterSendMailsModelValidator>().As<IValidator<NewsLetterSendMailsModel>>().SingleInstance();

            builder.RegisterType<NewsLetterValidator>().As<IValidator<NewsLetter>>().SingleInstance();

            builder.RegisterType<ResetPasswordDtoValidator>().As<IValidator<ResetPasswordDto>>().SingleInstance();

            builder.RegisterType<UserSignUpDtoValidator>().As<IValidator<UserSignUpDto>>().SingleInstance();

            builder.RegisterType<UserDtoValidator>().As<IValidator<UserDto>>().SingleInstance();
        }

    }
}
