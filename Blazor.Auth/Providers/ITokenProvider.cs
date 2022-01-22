using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Auth.Providers
{
    public interface ITokenProvider
    {
        ValueTask<string> GetTokenAsync();
    }
}
