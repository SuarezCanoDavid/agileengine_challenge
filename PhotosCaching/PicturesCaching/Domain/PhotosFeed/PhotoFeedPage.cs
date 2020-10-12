using Newtonsoft.Json;
using System.Collections.Generic;

namespace PicturesCaching.Domain.PhotosFeed
{
    public class PhotoFeedPage
    {
        [JsonProperty("pictures")]
        public IEnumerable<Picture> Pictures { get; set; }

        [JsonProperty("page")]
        public int PageNumber { get; set; }

        [JsonProperty("pageCount")]
        public int PageCount { get; set; }

        [JsonProperty("hasMore")]
        public bool HasMorePages { get; set; }
    }
}
