using Blazor.Loading.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Blazor.Loading.Services
{
    public interface ILayoutLoader
    {
        Task<AssemblyLayout> LoadLayoutAsync(string configKey, ILogger logger);
    }
}
