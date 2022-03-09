using Autofac;
using BusinessLayer.ValidationRules;
using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DependencyResolvers.Autofac
{
    public class AutofacValidationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BlogValidator>().As<IValidator<Blog>>().SingleInstance();

            builder.RegisterType<CategoryValidator>().As<IValidator<Category>>().SingleInstance();

            builder.RegisterType<ContactValidator>().As<IValidator<Contact>>().SingleInstance();

            builder.RegisterType<WriterValidator>().As<IValidator<Writer>>().SingleInstance();
        }
    }
}
