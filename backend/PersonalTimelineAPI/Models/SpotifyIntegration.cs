public class SpotifyIntegration
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }  
    public DateTime? TokenExpiresAt { get; set; }  
    public DateTime ConnectedAt { get; set; }
    public DateTime? LastSyncedAt { get; set; }
}