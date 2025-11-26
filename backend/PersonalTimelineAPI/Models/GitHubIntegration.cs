namespace PersonalTimelineAPI.Models;

public class GitHubIntegration
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string GitHubUsername { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public DateTime ConnectedAt { get; set; }
    public DateTime? LastSyncedAt { get; set; }
    
    public User? User { get; set; }
}