using Newtonsoft.Json;
using System;

namespace PhotosCaching.Domain.PhotosFeed
{
    public class PictureDetails : Picture, IEquatable<PictureDetails>
    {
        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("camera")]
        public string Camera { get; set; }

        [JsonProperty("tags")]
        public string Tags { get; set; }

        [JsonProperty("full_picture")]
        public string FullPictureUrl { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as PictureDetails);
        }

        public bool Equals(PictureDetails other)
        {
            return other != null &&
                   Id == other.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
