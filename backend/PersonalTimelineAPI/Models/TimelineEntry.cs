namespace PersonalTimelineAPI.Models
{
    public class TimelineEntry
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string EntryType { get; set; } = "Activity"; 
        public string Category { get; set; } = "General";
        public string ImageUrl { get; set; } = string.Empty;
        public string ExternalUrl { get; set; } = string.Empty;
        public string SourceApi { get; set; } = "Manual"; 
        public string ExternalId { get; set; } = string.Empty; 
        public string Metadata { get; set; } = "{}"; 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation property
        public User? User { get; set; }
    }
}