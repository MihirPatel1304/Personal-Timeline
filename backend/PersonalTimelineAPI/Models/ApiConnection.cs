namespace PersonalTimelineAPI.Models
{
    public class ApiConnection
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ApiProvider { get; set; } = string.Empty; 
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenExpiresAt { get; set; }
        public DateTime LastSyncAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        public string Settings { get; set; } = "{}"; 
        
        // Navigation property
        public User? User { get; set; }
    }
}