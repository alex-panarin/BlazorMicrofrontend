using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Blazor.Loading.Services
{
    public class RegisterServiceProvider : IServiceProvider, IServiceScopeFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<Type, object> _objects = new ConcurrentDictionary<Type, object>();
        public RegisterServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IServiceScope CreateScope()
        {
            return new RegisterServiceScope(this);
        }

        public ConcurrentDictionary<Type, Type> Services { get; } = new ConcurrentDictionary<Type, Type>();

        public object GetService(Type serviceType)
        {
            Console.WriteLine($"Get Service: {serviceType}");
            if (serviceType == typeof(IServiceScopeFactory))
                return this;
            // Find out service withing design time registration storage
            var service = _serviceProvider.GetService(serviceType);
            if (service != null)
                return service;
            // If not, try to find registration type
            var implType = Services.GetOrAdd(serviceType, service?.GetType());
            if (implType == null)
                return null;

            Console.WriteLine($"Find Registered Service: {serviceType}");
            // Get runtime registered service instance
            return _objects.GetOrAdd(serviceType, GetServiceImpl(implType));
        }
        
        private object GetServiceImpl(Type implementationType)
        {
            var constructor = implementationType
                .GetConstructors()
                .FirstOrDefault();
            var parameters = constructor?.GetParameters();
            var service = constructor?.Invoke(parameters?.Select(p => GetService(p.ParameterType)).ToArray());
            Console.WriteLine($"Create service: {service}");

            return service;
        }
    }

    class RegisterServiceScope : IServiceScope
    {
        public RegisterServiceScope(IServiceProvider provider)
        {
            Console.WriteLine($"Get Scope: {this}");
            ServiceProvider = provider;
        }
        public IServiceProvider ServiceProvider { get; private set; }

        public void Dispose()
        {
            ServiceProvider = null;
        }
    }
}