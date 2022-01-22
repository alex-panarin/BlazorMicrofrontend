using Blazor.Auth.Bootstrap;
using Blazor.Loading;
using Blazor.Loading.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
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
            builder.Services.AddOptions();
            builder.Services.AddAuthInfrastructure();
            var host = builder.Build();
            DefaultServiceProvider = host.Services;
            //var loader = DefaultServiceProvider.GetServices<ILoadingService>();
            //Console.WriteLine($"Loader: =>> {loader} <==");
            await host.RunAsync();

        }
    }
}
