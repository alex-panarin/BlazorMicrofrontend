using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;

namespace Blazor.Main.Services
{
    public interface INavigationService
    {
        bool IsNavigateTo(string url);
    }
}
