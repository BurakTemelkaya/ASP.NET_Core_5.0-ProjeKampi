using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddSession();

            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddMvc();
            services.AddAuthentication(
                CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(x =>
                {
                    x.LoginPath = "/Login/Index";
                }
            );

            services.AddSingleton<IAboutDal>(new EfAboutRepository());

            services.AddSingleton<IBlogDal>(new EfBlogRepository());

            services.AddSingleton<ICategoryDal>(new EfCategoryRepository());

            services.AddSingleton<ICommentDal>(new EfCommentRepository());

            services.AddSingleton<IContactDal>(new EfContactRepository());

            services.AddSingleton<IMessage2Dal>(new EfMessage2Repository());

            services.AddSingleton<INewsLetterDal>(new EfNewsLetterRepository());

            services.AddSingleton<INotificationDal>(new EfNotificationRepository());

            services.AddSingleton<IWriterDal>(new EfWriterRepository());

            services.AddSingleton<IAdminDal>(new EfAdminRepository());


            services.AddSingleton<IAboutService>(new AboutManager(new EfAboutRepository()));

            services.AddSingleton<IBlogService>(new BlogManager(new EfBlogRepository()));

            services.AddSingleton<ICategoryService>(new CategoryManager(new EfCategoryRepository()));

            services.AddSingleton<ICommentService>(new CommentManager(new EfCommentRepository()));

            services.AddSingleton<IContactService>(new ContactManager(new EfContactRepository()));

            services.AddSingleton<IMessage2Service>(new Message2Manager(new EfMessage2Repository()));

            services.AddSingleton<INewsLetterService>(new NewsLetterManager(new EfNewsLetterRepository()));

            services.AddSingleton<INotificationService>(new NotificationManager(new EfNotificationRepository()));

            services.AddSingleton<IWriterService>(new WriterManager(new EfWriterRepository()));

            services.AddSingleton<IAdminService>(new AdminManager(new EfAdminRepository()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseStatusCodePagesWithReExecute("/ErrorPage/Error404", "?code={0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {


                //endpoints.MapControllerRoute(
                //    name: "Admin",
                //    pattern: "{area:exist}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                    name: "Admin",
                    areaName: "Admin",
                    pattern: "/admin/{controller=Home}/{action=Index}/{id?}"
                    );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                
            });


        }
    }
}
