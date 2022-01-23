using Blazor.Services.Models;
using System.Threading.Tasks;

namespace Blazor.Services.Services
{
    public interface IReadPhotoRepository
    {
        Task<Photo[]> GetPhotos();
    }
}
