using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PhotosCaching.DataAccess;
using System.Linq;
using System.Threading.Tasks;

namespace PhotosCaching.Functions
{
    public class SearchPhotos
    {
        private readonly ICacheDbContext cacheDbContext;

        public SearchPhotos(ICacheDbContext cacheDbContext)
        {
            this.cacheDbContext = cacheDbContext;
        }

        [FunctionName("SearchPhotos")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "search/{searchTerm}")] HttpRequest req, string searchTerm,
            ILogger log)
        {
            var term = searchTerm.ToLower();

            var photos = from photo in await cacheDbContext.PictureDetails.ToArrayAsync()
                         where photo.Author.ToLower().Contains(term)
                         || photo.Camera.ToLower().Contains(term)
                         || photo.Tags.ToLower().Contains(term)
                         select photo;

            return new OkObjectResult(photos);
        }
    }
}
