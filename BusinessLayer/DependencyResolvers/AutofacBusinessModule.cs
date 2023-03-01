using Autofac;
using Autofac.Extras.DynamicProxy;
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using Castle.DynamicProxy;
using CoreLayer.Utilities.Interceptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DependencyResolvers
{
    public class AutofacBusinessModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AboutManager>().As<IAboutService>().SingleInstance();

            builder.RegisterType<BlogManager>().As<IBlogService>().SingleInstance();

            builder.RegisterType<UserBusinessManager>().As<IBusinessUserService>().SingleInstance();

            builder.RegisterType<CategoryManager>().As<ICategoryService>().SingleInstance();

            builder.RegisterType<CommentManager>().As<ICommentService>().SingleInstance();

            builder.RegisterType<ContactManager>().As<IContactService>().SingleInstance();

            builder.RegisterType<MessageDraftManager>().As<IMessageDraftService>().SingleInstance();

            builder.RegisterType<MessageManager>().As<IMessageService>().SingleInstance();

            builder.RegisterType<NewsLetterDraftManager>().As<INewsLetterDraftService>().SingleInstance();

            builder.RegisterType<NewsLetterManager>().As<INewsLetterService>().SingleInstance();

            builder.RegisterType<NotificationManager>().As<INotificationService>().SingleInstance();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();
        }
    }
}
