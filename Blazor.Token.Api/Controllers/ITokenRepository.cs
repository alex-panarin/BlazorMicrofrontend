using Blazor.Token.Api.Model;
using System.Threading.Tasks;

namespace Blazor.Token.Api.Controllers
{
    public interface ITokenRepository
    {
        Task CreateAsync(TokenRequest token);
        Task<TokenResponse> VerifyAsync(TokenRequest token);
        Task<string> RefreshAsync(string token);
    }
}