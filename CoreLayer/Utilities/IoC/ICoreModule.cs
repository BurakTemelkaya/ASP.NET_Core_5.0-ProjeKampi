using Microsoft.Extensions.DependencyInjection;

namespace CoreLayer.Utilities.IoC
{
    public interface ICoreModule
    {
        void Load(IServiceCollection serviceCollection);
    }
}
