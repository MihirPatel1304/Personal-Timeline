using System.Text.Json.Serialization;

namespace YourProjectName.Models
{
    public class SpotifyTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
        
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
    }
}