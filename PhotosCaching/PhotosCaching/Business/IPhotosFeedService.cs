using PhotosCaching.Domain.PhotosFeed;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotosCaching.Business
{
    public interface IPhotosFeedService
    {
        Task<IEnumerable<PictureDetails>> GetPhotosAsync();
    }
}
