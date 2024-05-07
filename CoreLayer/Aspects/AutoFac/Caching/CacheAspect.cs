using Castle.DynamicProxy;
using CoreLayer.Aspects.AutoFac.Performance;
using CoreLayer.CrossCuttingConcerns.Caching;
using CoreLayer.Utilities.Interceptors;
using CoreLayer.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Linq;

namespace CoreLayer.Aspects.AutoFac.Caching
{
    public class CacheAspect : MethodInterception
    {
        private readonly int _duration;
        private readonly ICacheManager _cacheManager;

        public CacheAspect(int duration = 3600)
        {
            _duration = duration;
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
        }

        public override void Intercept(IInvocation invocation)
        {
            var methodName = $"{invocation.Method.ReflectedType.FullName}.{invocation.Method.Name}";

            var arguments = invocation.Arguments.Select(arg => arg != null && arg.GetType().IsClass 
            ? JsonConvert.SerializeObject(arg, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }) 
            : arg?.ToString() ?? "<Null>").ToList();

            var key = $"{methodName}({string.Join(",", arguments)})";

            if (_cacheManager.IsAdd(key))
            {
                invocation.ReturnValue = _cacheManager.Get(key);
                return;
            }
            invocation.Proceed();
            _cacheManager.Add(key, invocation.ReturnValue, _duration);
        }
    }
}
