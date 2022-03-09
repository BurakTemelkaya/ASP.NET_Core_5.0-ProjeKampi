using Autofac;
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EfCategoryRepository>().As<ICategoryDal>().SingleInstance();

            builder.RegisterType<EfAboutRepository>().As<IAboutDal>().SingleInstance();

            builder.RegisterType<EfAdminRepository>().As<IAdminDal>().SingleInstance();

            builder.RegisterType<EfBlogRepository>().As<IBlogDal>().SingleInstance();

            builder.RegisterType<EfCommentRepository>().As<ICommentDal>().SingleInstance();

            builder.RegisterType<EfContactRepository>().As<IContactDal>().SingleInstance();

            builder.RegisterType<EfMessage2Repository>().As<IMessage2Dal>().SingleInstance();

            builder.RegisterType<EfNewsLetterRepository>().As<INewsLetterDal>().SingleInstance();

            builder.RegisterType<EfNotificationRepository>().As<INotificationDal>().SingleInstance();

            builder.RegisterType<EfWriterRepository>().As<IWriterDal>().SingleInstance();


            builder.RegisterType<AboutManager>().As<IAboutService>().SingleInstance();

            builder.RegisterType<AdminManager>().As<IAdminService>().SingleInstance();

            builder.RegisterType<BlogManager>().As<IBlogService>().SingleInstance();

            builder.RegisterType<CategoryManager>().As<ICategoryService>().SingleInstance();

            builder.RegisterType<CommentManager>().As<ICommentService>().SingleInstance();

            builder.RegisterType<ContactManager>().As<IContactService>().SingleInstance();

            builder.RegisterType<Message2Manager>().As<IMessage2Service>().SingleInstance();

            builder.RegisterType<NewsLetterManager>().As<INewsLetterService>().SingleInstance();

            builder.RegisterType<NotificationManager>().As<INotificationService>().SingleInstance();

            builder.RegisterType<WriterManager>().As<IWriterService>().SingleInstance();
        }
    }
}
