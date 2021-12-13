using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DependencyResolvers.Ninject
{
    public class BusinessModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IAboutDal>().To<EfAboutRepository>().InSingletonScope();

            Bind<IAdminDal>().To<EfAdminRepository>().InSingletonScope();

            Bind<IBlogDal>().To<EfBlogRepository>().InSingletonScope();

            Bind<ICategoryDal>().To<EfCategoryRepository>().InSingletonScope();

            Bind<ICommentDal>().To<EfCommentRepository>().InSingletonScope();

            Bind<IContactDal>().To<EfContactRepository>().InSingletonScope();

            Bind<IMessage2Dal>().To<EfMessage2Repository>().InSingletonScope();

            Bind<INewsLetterDal>().To<EfNewsLetterRepository>().InSingletonScope();

            Bind<INotificationDal>().To<EfNotificationRepository>().InSingletonScope();

            Bind<IWriterDal>().To<EfWriterRepository>().InSingletonScope();

           

            Bind<IAboutService>().To<AboutManager>().InSingletonScope();

            Bind<IAdminService>().To<AdminManager>().InSingletonScope();

            Bind<IBlogService>().To<BlogManager>().InSingletonScope();

            Bind<ICategoryService>().To<CategoryManager>().InSingletonScope();

            Bind<ICommentService>().To<CommentManager>().InSingletonScope();

            Bind<IContactService>().To<ContactManager>().InSingletonScope();

            Bind<IMessage2Service>().To<Message2Manager>().InSingletonScope();

            Bind<INewsLetterService>().To<NewsLetterManager>().InSingletonScope();

            Bind<INotificationService>().To<NotificationManager>().InSingletonScope();

            Bind<IWriterService>().To<WriterManager>().InSingletonScope();


        }
    }
}
