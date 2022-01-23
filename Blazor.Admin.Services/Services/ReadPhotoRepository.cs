using Blazor.Register;
using Blazor.Services.Models;
using System.Threading.Tasks;

namespace Blazor.Services.Services
{
    [Registration(RegistrationType =typeof(IReadPhotoRepository))]
    public class ReadPhotoRepository : IReadPhotoRepository
    {
        public Task<Photo[]> GetPhotos()
        {
            return Task.FromResult(new[]
            {
                new Photo{ Title = "Cloud over a frozen lake",
                    Url="https://media.istockphoto.com/photos/cloud-over-a-frozen-lake-picture-id921594468?b=1&k=20&m=921594468&s=170667a&w=0&h=ATXofMRU5gGO9imLyhps51iN33mD6RldjCoW2DsWz3M="},
                new Photo{ Title = "Christmas decoration", 
                    Url="https://media.istockphoto.com/photos/christmas-decoration-bauble-outdoors-in-nature-on-fir-tree-christmas-picture-id1354036772?b=1&k=20&m=1354036772&s=170667a&w=0&h=0QTqw8bvtwcxYWrrOHVNz0F8nXpBfc9FQaW5ldjcZrU="}
            });
        }
    }
}
