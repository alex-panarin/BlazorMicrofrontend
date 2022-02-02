using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Loading.Services
{
    public interface ILoadingService
    {
        List<Assembly> LoadedAssemblies { get; }
        Task Loading(string contextKey, ILogger logger);
    }
}
