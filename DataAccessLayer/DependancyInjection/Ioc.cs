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

            services.AddScoped<ICategoryDal, EfCategoryRepository>();

            services.AddScoped<IAboutDal, EfAboutRepository>();

            services.AddScoped<IBlogDal, EfBlogRepository>();

            services.AddScoped<ICommentDal, EfCommentRepository>();

            services.AddScoped<IContactDal, EfContactRepository>();

            services.AddScoped<IMessageDal, EfMessageRepository>();

            services.AddScoped<INewsLetterDal, EfNewsLetterRepository>();

            services.AddScoped<INotificationDal, EfNotificationRepository>();

            services.AddScoped<IMessageDraftDal, EfMessageDraftRepository>();

            services.AddScoped<INewsLetterDraftDal, EfNewsLetterDraftRepository>();
        }
    }
}
