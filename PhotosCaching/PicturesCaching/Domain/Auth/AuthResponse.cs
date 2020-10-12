using Newtonsoft.Json;

namespace PicturesCaching.Domain.Auth
{
    public class AuthResponse
    {
        [JsonProperty("auth")]
        public bool IsAuthorized { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
