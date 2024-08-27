using CoreLayer.Utilities.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDependencyResolvers
        (this IServiceCollection serviceCollection, IConfiguration configuration, ICoreModule[] modules)
    {
        foreach (var module in modules)
        {
            module.Load(serviceCollection, configuration);
        }

        return ServiceTool.Create(serviceCollection);
    }
}
