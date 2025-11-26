namespace PersonalTimelineAPI.DTOs
{
    public class SpotifyStatus
    {
        public bool Connected { get; set; }
        public string? DisplayName { get; set; }
        public DateTime? LastSynced { get; set; }
    }

    public class SpotifySyncResult
    {
        public int NewEntries { get; set; }
    }

    public class SpotifyConnectResult
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
    }
}
