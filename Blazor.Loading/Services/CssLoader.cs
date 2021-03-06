using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Blazor.Loading.Services
{
    public class CssLoader : ICssLoader, IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;
        public CssLoader(IJSRuntime jS,
            IConfiguration configuration)
        {
            var js = jS ?? throw new ArgumentNullException(nameof(jS));
            var _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            var jsUrl = $"{_configuration.GetSection("BaseAddress").Value}cssLoad.js";
            moduleTask = new(() => js.InvokeAsync<IJSObjectReference>(
                "import", jsUrl).AsTask());
        }

        public async ValueTask LoadCssAsync(string[] cssNames)
        {
            var module = await moduleTask.Value;
            foreach (var cssName in cssNames)
            {
                var cssUrl = $"{cssName}.css";
                await module.InvokeVoidAsync("loadCss", cssUrl);
            }
            
            Console.WriteLine("Css load call from cs");
        }
        public async ValueTask DisposeAsync()
        {
            if(moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}
