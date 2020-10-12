using Newtonsoft.Json;

namespace PhotosCaching.Domain.Auth
{
    public class AuthResponse
    {
        [JsonProperty("auth")]
        public bool IsAuthorized { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
