using Microsoft.AspNetCore.Mvc.Formatters.Internal;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PhotosCaching.Business;
using PhotosCaching.DataAccess;
using PhotosCaching.Domain.PhotosFeed;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotosCaching.Functions
{
    public class RefreshPhotos
    {
        private readonly IPhotosFeedService photosFeedService;
        private readonly ICacheDbContext cacheDbContext;

        public RefreshPhotos(IPhotosFeedService photosFeedService, ICacheDbContext cacheDbContext)
        {
            this.photosFeedService = photosFeedService;
            this.cacheDbContext = cacheDbContext;
        }

        [FunctionName("RefreshPhotos")]
        public async Task Run([TimerTrigger("*/15 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            var cachedPhotos = await cacheDbContext.PictureDetails.ToArrayAsync();
            var fetchedPhotos = await photosFeedService.GetPhotosAsync();

            await AddNewPhotosAsync(fetchedPhotos, cachedPhotos);
            await DeleteOldPhotosAsync(fetchedPhotos, cachedPhotos);
            await UpdatePhotosAsync(fetchedPhotos, cachedPhotos);

            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }

        private async Task AddNewPhotosAsync(IEnumerable<PictureDetails> fetched, IEnumerable<PictureDetails> cached)
        {
            var photosToAdd = fetched.Except(cached);

            cacheDbContext.PictureDetails.AddRange(photosToAdd);

            await cacheDbContext.SaveChangesAsync();
        }

        private async Task DeleteOldPhotosAsync(IEnumerable<PictureDetails> fetched, IEnumerable<PictureDetails> cached)
        {
            var photosToDelete = cached.Except(fetched);

            cacheDbContext.PictureDetails.RemoveRange(photosToDelete);

            await cacheDbContext.SaveChangesAsync();
        }

        private async Task UpdatePhotosAsync(IEnumerable<PictureDetails> fetched, IEnumerable<PictureDetails> cached)
        {
            var photosToUpdate = from f in fetched
                                 join c in cached on f.Id equals c.Id
                                 where f.Author != c.Author || f.Camera != c.Camera || f.Tags != c.Tags || f.CroppedPictureUrl != c.CroppedPictureUrl || f.FullPictureUrl != c.FullPictureUrl
                                 select new { Fetched = f, Cached = c };

            foreach (var pair in photosToUpdate)
            {
                pair.Cached.Author = pair.Fetched.Author;
                pair.Cached.Camera = pair.Fetched.Camera;
                pair.Cached.Tags = pair.Fetched.Tags;
                pair.Cached.CroppedPictureUrl = pair.Fetched.CroppedPictureUrl;
                pair.Cached.FullPictureUrl = pair.Fetched.FullPictureUrl;
            }

            await cacheDbContext.SaveChangesAsync();
        }
    }
}
