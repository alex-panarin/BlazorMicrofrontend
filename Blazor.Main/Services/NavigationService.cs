using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Blazor.Main.Services
{
    public class NavigationService : INavigationService
    {
        public bool IsNavigateTo(string url)
        {
            return url == "admin" || url == "guest";
        }
    }
}
