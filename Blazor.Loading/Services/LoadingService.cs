using Blazor.Loading.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Blazor.Loading.Services
{
    public class LoadingService : ILoadingService
    {
        private readonly ILayoutLoader _layoutLoader;
        private readonly IAssembliesLoader _assembliesLoader;
        private readonly ICssLoader _cssLoader;
        private static readonly ConcurrentDictionary<string, AssemblyLayout> _layouts = new ConcurrentDictionary<string, AssemblyLayout>();
        public List<Assembly> LoadedAssemblies { get; } = new List<Assembly> { };

        public LoadingService(ILayoutLoader layoutLoader, 
            IAssembliesLoader assembliesLoader,
            ICssLoader cssLoader)
        {
            _layoutLoader = layoutLoader ?? throw new ArgumentNullException(nameof(layoutLoader));
            _assembliesLoader = assembliesLoader ?? throw new ArgumentNullException(nameof(assembliesLoader)); 
            _cssLoader = cssLoader ?? throw new ArgumentNullException(nameof(cssLoader));
        }
        public async Task Loading(string contextKey, IServiceProvider serviceProvider, ILogger logger = null)
        {
            // Find out layout and associated assemblies and load assemblies in to application context
            logger?.LogInformation($"Loading context: {contextKey}");
            LoadedAssemblies.Clear();

            var layout = _layouts.GetOrAdd(contextKey, await _layoutLoader.LoadLayoutAsync(contextKey, logger));
            if (layout == null)
                return;

            var assemblies = await _assembliesLoader.LoadAssembliesAsync(layout.Assemblies, serviceProvider, logger);
            LoadedAssemblies.AddRange(assemblies);

            await _cssLoader.LoadCssAsync(layout.Css);
        }
    }
}
