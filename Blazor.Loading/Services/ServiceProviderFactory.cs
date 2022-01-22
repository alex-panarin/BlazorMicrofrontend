using Microsoft.Extensions.DependencyInjection;
using System;

namespace Blazor.Loading.Services
{
    public class ServiceProviderFactory : IServiceProviderFactory<IServiceCollection>
    {
        private readonly ServiceProviderOptions _options;
        private RegisterServiceProvider _provider;
        public ServiceProviderFactory()
        {
            _options = new ServiceProviderOptions();
        }
        public IServiceCollection CreateBuilder(IServiceCollection services)
        {
            return services;
        }

        public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder)
        {
            if (_provider == null)
                _provider = new RegisterServiceProvider(containerBuilder.BuildServiceProvider(_options));
            return _provider;
        }
    }
}
