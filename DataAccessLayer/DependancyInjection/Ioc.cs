using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.Concrete.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DependancyInjection
{
    public static class Ioc
    {
        public static void IocDataAccessInstall(this IServiceCollection services)
        {

            services.AddTransient<ICategoryDal, EfCategoryRepository>();

            services.AddTransient<IAboutDal, EfAboutRepository>();

            services.AddTransient<ICategoryDal, EfCategoryRepository>();

            services.AddTransient<IBlogDal, EfBlogRepository>();

            services.AddTransient<ICommentDal, EfCommentRepository>();

            services.AddTransient<IContactDal, EfContactRepository>();

            services.AddTransient<IMessageDal, EfMessageRepository>();

            services.AddTransient<INewsLetterDal, EfNewsLetterRepository>();

            services.AddTransient<INotificationDal, EfNotificationRepository>();

            services.AddTransient<IMessageDraftDal, EfMessageDraftRepository>();

            services.AddTransient<INewsLetterDraftDal, EfNewsLetterDraftRepository>();
        }
    }
}
