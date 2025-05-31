using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using CoreLayer.Utilities.Interceptors;

namespace BusinessLayer.DependencyResolvers;

public class AutofacBusinessModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var assembly = System.Reflection.Assembly.GetExecutingAssembly();

        builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
            .EnableInterfaceInterceptors(new ProxyGenerationOptions()
            {
                Selector = new AspectInterceptorSelector()
            }).InstancePerDependency();
    }
}
