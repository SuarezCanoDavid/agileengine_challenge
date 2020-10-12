using PicturesCaching.Domain.PhotosFeed;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PicturesCaching.Business
{
    public interface IPhotosFeedService
    {
        Task<IEnumerable<PictureDetails>> GetPhotosAsync();
    }
}
