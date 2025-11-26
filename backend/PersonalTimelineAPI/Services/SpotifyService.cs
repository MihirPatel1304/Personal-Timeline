using PersonalTimelineAPI.DTOs;
using System.Net.Http.Headers;
using System.Text.Json;

namespace PersonalTimelineAPI.Services
{
    public class SpotifyService
    {
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;

        public SpotifyService(IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<SpotifyConnectResult> ConnectAsync(string code, int userId)
        {
            var clientId = _config["Spotify:ClientId"];
            var clientSecret = _config["Spotify:ClientSecret"];

            var httpClient = _httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");
            var authHeader = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeader);

            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", "https://localhost:5173/auth/spotify/callback" }
            });
            request.Content = content;

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var tokenResult = JsonSerializer.Deserialize<SpotifyConnectResult>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // TODO: Save tokens to your DB for the userId

            return tokenResult!;
        }

        public async Task<SpotifySyncResult> SyncAsync(int userId)
        {
            // TODO: Use stored access token to fetch user top tracks / recently played tracks
            // Save new entries to DB
            return new SpotifySyncResult { NewEntries = 0 };
        }

        public async Task<SpotifyStatus> GetStatusAsync(int userId)
        {
            // TODO: Check DB if user has connected Spotify
            return new SpotifyStatus
            {
                Connected = false,
                DisplayName = null,
                LastSynced = null
            };
        }

        public async Task DisconnectAsync(int userId)
        {
            // TODO: Remove tokens from DB
        }
    }
}
