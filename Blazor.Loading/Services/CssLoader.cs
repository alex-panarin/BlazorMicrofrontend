using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Blazor.Loading.Services
{
    public class CssLoader : ICssLoader, IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;
        public CssLoader(IJSRuntime jS)
        {
            var js = jS ?? throw new ArgumentNullException(nameof(jS));
            moduleTask = new(() => js.InvokeAsync<IJSObjectReference>(
                "import", "https://localhost:44385/cssLoad.js").AsTask());
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
