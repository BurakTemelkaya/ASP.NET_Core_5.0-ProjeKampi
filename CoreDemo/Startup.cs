using BusinessLayer.AutoMapper.Profiles;
using BusinessLayer.DependencyResolvers;
using BusinessLayer.Extension;
using BusinessLayer.ValidationRules;
using Core.Extensions;
using CoreDemo.AutoMapper.Profiles;
using CoreDemo.Models;
using CoreLayer.DependancyResolvers;
using CoreLayer.Utilities.IoC;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

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
            services.AddDbContext<Context>();
            services.AddIdentity<AppUser, AppRole>(x =>
            {
                x.User.RequireUniqueEmail = true;
                x.Password.RequireUppercase = true;
                x.Password.RequireNonAlphanumeric = true;
                x.Password.RequireDigit = true;
                x.Password.RequireLowercase = true;
                x.Password.RequireNonAlphanumeric = true;
            }).AddEntityFrameworkStores<Context>();

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

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(100);
                options.LoginPath = "/Login/Index";
                options.LogoutPath = "/Login/Logout";
                options.AccessDeniedPath = new PathString("/Login/AccessDenied");
                options.SlidingExpiration = true;
            });

            services.AddSingleton(new UserInfo());

            services.AddSingleton(new WriterCity());

            services.AddDependencyResolvers(new ICoreModule[] {
               new CoreModule()
            });

            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();

            services.AddAutoMapper(typeof(BusinessImages));
            services.AddAutoMapper(typeof(UIImage));

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

            app.UseUserDestroyer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "Admin",
                    areaName: "Admin",
                    pattern: "/admin/{controller=Home}/{action=Index}/{id?}"
                    );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Blog}/{action=Index}/{id?}");              
            });


        }
    }
}
