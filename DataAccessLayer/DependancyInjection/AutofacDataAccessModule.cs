using Autofac;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.EntityFramework;

namespace DataAccessLayer.DependancyInjection
{
    public class AutofacDataAccessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EfCategoryRepository>().As<ICategoryDal>().SingleInstance();

            builder.RegisterType<EfAboutRepository>().As<IAboutDal>().SingleInstance();

            builder.RegisterType<EfBlogRepository>().As<IBlogDal>().SingleInstance();

            builder.RegisterType<EfCommentRepository>().As<ICommentDal>().SingleInstance();

            builder.RegisterType<EfContactRepository>().As<IContactDal>().SingleInstance();

            builder.RegisterType<EfMessageRepository>().As<IMessageDal>().SingleInstance();

            builder.RegisterType<EfNewsLetterRepository>().As<INewsLetterDal>().SingleInstance();

            builder.RegisterType<EfNotificationRepository>().As<INotificationDal>().SingleInstance();

            builder.RegisterType<EfMessageDraftRepository>().As<IMessageDraftDal>().SingleInstance();

            builder.RegisterType<EfNewsLetterDraftRepository>().As<INewsLetterDraftDal>().SingleInstance();
        }
    }
}
