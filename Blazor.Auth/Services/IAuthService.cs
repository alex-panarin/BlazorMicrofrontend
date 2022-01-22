using Blazor.Auth.Models;
using System.Threading.Tasks;

namespace Blazor.Auth.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(LoginRequest request);
        Task Logout();
        Task<RegisterResponse> Register(RegisterRequest request);
    }
}
