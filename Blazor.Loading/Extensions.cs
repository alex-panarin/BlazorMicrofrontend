using Blazor.Loading.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Blazor.Loading
{
    public static  class Extensions
    {
        public static void AddAssebliesLoader(this IServiceCollection services)
        {
            services.AddSingleton<ILayoutLoader, LayoutLoader>();
            services.AddSingleton<IAssembliesLoader, AssembliesLoader>();
            services.AddSingleton<ILoadingService, LoadingService>();
        }
        public static void RegisterService(this IServiceProvider provider, Type serviceType, Type implementationType)
        {
            (provider as RegisterServiceProvider)?
                .Services
                .AddOrUpdate(serviceType, implementationType, (k, v) => v);
        }
    }
}
