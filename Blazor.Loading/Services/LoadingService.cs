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
        private static readonly ConcurrentDictionary<string, AssemblyLayout> _layouts = new ConcurrentDictionary<string, AssemblyLayout>();
        public List<Assembly> LoadedAssemblies { get; } = new List<Assembly> { };

        public LoadingService(ILayoutLoader layoutLoader, 
            IAssembliesLoader assembliesLoader)
        {
            _layoutLoader = layoutLoader ?? throw new ArgumentNullException(nameof(layoutLoader));
            _assembliesLoader = assembliesLoader ?? throw new ArgumentNullException(nameof(assembliesLoader)); 
        }
        public async Task Loading(string contextPath, IServiceProvider serviceProvider, ILogger logger = null)
        {
            logger?.LogInformation($"Loading {contextPath}");
            LoadedAssemblies.Clear();

            var layout = _layouts.GetOrAdd(contextPath, await _layoutLoader.LoadLayoutAsync(contextPath, logger));
            if (layout == null)
                return;

            var assemblies = await _assembliesLoader.LoadAssembliesAsync(layout.Assemblies, serviceProvider, logger);
            LoadedAssemblies.AddRange(assemblies);
        }
    }
}
