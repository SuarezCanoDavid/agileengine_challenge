using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PhotosCaching.Business.Configuration;
using PhotosCaching.Domain.Auth;
using PhotosCaching.Domain.PhotosFeed;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PhotosCaching.Business
{
    public class PhotosFeedService : IPhotosFeedService
    {
        private readonly HttpClient httpClient;
        private readonly PhotosFeedOptions photosFeedOptions;

        public PhotosFeedService(HttpClient httpClient, IOptionsMonitor<PhotosFeedOptions> photosFeedOptionsAccessor)
        {
            this.httpClient = httpClient;
            this.photosFeedOptions = photosFeedOptionsAccessor.CurrentValue;
        }

        public async Task<IEnumerable<PictureDetails>> GetPhotosAsync()
        {
            var page = default(PhotoFeedPage);
            var photos = new List<PictureDetails>();
            var token = await GetBearerTokenAsync();
            var pageNumber = 0;

            do
            {
                pageNumber++;

                page = await GetPhotoFeedPageAsync(pageNumber, token);

                foreach (var id in page.Pictures.Select(_ => _.Id))
                {
                    var detail = await GetPictureDetailsAsync(id, token);
                    photos.Add(detail);
                }
            } 
            while (page.HasMorePages);

            return photos;
        }

        private async Task<string> GetBearerTokenAsync()
        {
            var body = JsonConvert.SerializeObject(new { apiKey = photosFeedOptions.ApiKey });
            var requestContent = new StringContent(body, Encoding.UTF8, "application/json");

            using (var response = await httpClient.PostAsync("/auth", requestContent))
            {
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var authResponse = JsonConvert.DeserializeObject<AuthResponse>(responseContent);

                    if (authResponse.IsAuthorized)
                        return authResponse.Token;
                }

                return null;
            }
        }

        private async Task<PhotoFeedPage> GetPhotoFeedPageAsync(int pageNumber, string bearerToken) => await GetAsync<PhotoFeedPage>($"/images?page={pageNumber}", bearerToken);

        private async Task<PictureDetails> GetPictureDetailsAsync(string id, string bearerToken) => await GetAsync<PictureDetails>($"/images/{id}", bearerToken);

        private async Task<T> GetAsync<T>(string url, string bearerToken) where T : class
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            using (var response = await httpClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<T>(responseContent);

                    return result;
                }

                return null;
            }
        }
    }
}
