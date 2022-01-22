using Microsoft.JSInterop;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Blazor.Auth.Services
{
    internal class LocalStorageService : ILocalStorageService
    {
        private readonly IJSRuntime _jSRuntime;
        private readonly IJSInProcessRuntime _jSInProcessRuntime;
        public LocalStorageService(IJSRuntime jSRuntime)
        {
            _jSRuntime = jSRuntime ?? throw new ArgumentNullException(nameof(jSRuntime));
            _jSInProcessRuntime = jSRuntime as IJSInProcessRuntime ?? throw new InvalidOperationException("IJSInProcessRuntime not available");                
        }

        public ValueTask<string> GetItemAsync(string key)
            => _jSRuntime.InvokeAsync<string>("localStorage.getItem", CancellationToken.None, key);

        public ValueTask RemoveItemAsync(string key)
            => _jSRuntime.InvokeVoidAsync("localStorage.removeItem", CancellationToken.None, key);

        public ValueTask SetItemAsync(string key, string data)
            => _jSRuntime.InvokeVoidAsync("localStorage.setItem", CancellationToken.None, key, data);

        public string GetItem(string key)
        {
            return _jSInProcessRuntime.Invoke<string>("localStorage.getItem", key);
        }

        public void RemoveItem(string key)
        {
            _jSInProcessRuntime.InvokeVoid("localStorage.removeItem", key);
        }
        public void SetItem(string key, string data)
        {
            _jSInProcessRuntime.InvokeVoid("localStorage.setItem", key, data);
        }
    }
}
