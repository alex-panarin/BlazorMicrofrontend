using Blazor.Auth.Bootstrap;
using Blazor.Loading;
using Blazor.Loading.Services;
using Blazor.Main.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Blazor.Main
{
    public class Program
    {
        public static IServiceProvider DefaultServiceProvider { get; private set; }

        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.ConfigureContainer(new ServiceProviderFactory());
            builder.RootComponents.Add<App>("#app");
            builder.Services.AddAssebliesLoader();
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Configuration.Add(new MemoryConfigurationSource 
            { 
                InitialData = new Dictionary<string, string> 
                { 
                    { "BaseAddress", builder.HostEnvironment.BaseAddress } 
                } 
            });
            builder.Services.AddOptions();
            builder.Services.AddAuthInfrastructure();
            builder.Services.AddScoped<INavigationService, NavigationService>();
            var host = builder.Build();
            await host.RunAsync();

        }
    }
}
