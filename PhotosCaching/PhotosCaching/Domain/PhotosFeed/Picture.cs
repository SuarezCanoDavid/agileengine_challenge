using Newtonsoft.Json;

namespace PhotosCaching.Domain.PhotosFeed
{
    public class Picture
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("cropped_picture")]
        public string CroppedPictureUrl { get; set; }
    }
}
