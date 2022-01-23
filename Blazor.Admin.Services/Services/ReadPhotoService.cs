using Blazor.Register;
using Blazor.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Services.Services
{
    [Registration(RegistrationType = typeof(IReadPhotoService))]
    public class ReadPhotoService : IReadPhotoService
    {
        private readonly IReadPhotoRepository _photoRepository;

        public ReadPhotoService(IReadPhotoRepository photoRepository)
        {
            _photoRepository = photoRepository ?? throw new ArgumentNullException(nameof(photoRepository));
        }
        public async Task<Photo> ReadPhoto(string photoId)
        {
            try
            {
                var photos = await _photoRepository.GetPhotos();

                return await Task.FromResult(photos.Where(p => p.id == photoId).FirstOrDefault());
            }
            catch (Exception)
            {
                throw ;
            }
        }

        public async Task<Photo[]> ReadPhotos()
        {
            try
            {
                return await _photoRepository.GetPhotos();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
