using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Loading.Services
{
    public interface ICssLoader
    {
        ValueTask LoadCssAsync(string[] cssNames);
        
    }
}
