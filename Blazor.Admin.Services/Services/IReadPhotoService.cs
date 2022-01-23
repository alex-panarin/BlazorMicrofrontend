using Blazor.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Services.Services
{
    public interface IReadPhotoService
    {
        Task<Photo> ReadPhoto(string photoId);
        Task<Photo[]> ReadPhotos();
    }
}
