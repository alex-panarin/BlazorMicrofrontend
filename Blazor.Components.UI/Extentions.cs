using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Components.UI
{
    public static class Extentions
    {
        public static async Task<bool> Confirm(this IJSRuntime js, string message)
        {
            return await js.InvokeAsync<bool>("confirm", message);
        }
    }
}
