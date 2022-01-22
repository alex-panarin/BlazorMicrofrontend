using Blazor.Loading.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Blazor.Loading.Services
{
    public class LayoutLoader : ILayoutLoader
    {
        private readonly HttpClient _client;
        public LayoutLoader(HttpClient client)
        {
            _client = client;
        }
        public async Task<AssemblyLayout> LoadLayoutAsync(string configKey, ILogger logger = null)
        {
            var array = await _client.GetFromJsonAsync<LayoutArray>("layout.json", new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            logger?.LogInformation($"==> Layouts: {string.Join("; ", array.Layouts.Select(l => l.Key))} <==");

            return array
                .Layouts
                .FirstOrDefault(l => string.Equals(l.Key, configKey, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
