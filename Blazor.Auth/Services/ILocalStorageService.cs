using System.Threading.Tasks;

namespace Blazor.Auth.Services
{
    public interface ILocalStorageService
    {
        ValueTask SetItemAsync(string key, string value); 
        ValueTask RemoveItemAsync(string key);

        ValueTask<string> GetItemAsync(string key);
    }
}
