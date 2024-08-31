using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer.DependancyInjection;

public static class Ioc
{
    public static void IocDataAccessInstall(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<Context>(options =>
            options.UseSqlServer(configuration.GetConnectionString("SQLServer")), ServiceLifetime.Scoped);

        services.AddScoped<ICategoryDal, EfCategoryRepository>();

        services.AddScoped<IAboutDal, EfAboutRepository>();

        services.AddScoped<IBlogDal, EfBlogRepository>();

        services.AddScoped<IBlogViewDal, EfBlogViewRepository>();

        services.AddScoped<ICommentDal, EfCommentRepository>();

        services.AddScoped<IContactDal, EfContactRepository>();

        services.AddScoped<IMessageDal, EfMessageRepository>();

        services.AddScoped<INewsLetterDal, EfNewsLetterRepository>();

        services.AddScoped<INotificationDal, EfNotificationRepository>();

        services.AddScoped<IMessageDraftDal, EfMessageDraftRepository>();

        services.AddScoped<INewsLetterDraftDal, EfNewsLetterDraftRepository>();

        services.AddScoped<ILogDal, EfLogRepository>();

        services.AddScoped<ILoginLoggerDal, EfLoginLoggerRepository>();
    }
}
