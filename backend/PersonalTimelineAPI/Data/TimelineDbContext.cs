using Microsoft.EntityFrameworkCore;
using PersonalTimelineAPI.Models;

namespace PersonalTimelineAPI.Data
{
    public class TimelineDbContext : DbContext
    {
        public TimelineDbContext(DbContextOptions<TimelineDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TimelineEntry> TimelineEntries { get; set; }
        public DbSet<ApiConnection> ApiConnections { get; set; }
        public DbSet<GitHubIntegration> GitHubIntegrations { get; set; }
        public DbSet<SpotifyIntegration> SpotifyIntegrations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.OAuthProvider).IsRequired();
                entity.Property(e => e.OAuthId).IsRequired();
                
                // One user has many timeline entries
                entity.HasMany(e => e.TimelineEntries)
                    .WithOne(e => e.User)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                // One user has many API connections
                entity.HasMany(e => e.ApiConnections)
                    .WithOne(e => e.User)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure TimelineEntry entity
            modelBuilder.Entity<TimelineEntry>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.EventDate).IsRequired();
            });

            // Configure ApiConnection entity
            modelBuilder.Entity<ApiConnection>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ApiProvider).IsRequired();
            });

            // Configure GitHubIntegration entity
            modelBuilder.Entity<GitHubIntegration>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.GitHubUsername).IsRequired();
                entity.Property(e => e.AccessToken).IsRequired();
                
                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Seed data for testing
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed a test user
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    OAuthProvider = "Google",
                    OAuthId = "test-oauth-id-123",
                    Email = "testuser@example.com",
                    DisplayName = "Test User",
                    ProfileImageUrl = "https://via.placeholder.com/150",
                    CreatedAt = DateTime.UtcNow,
                    LastLoginAt = DateTime.UtcNow
                }
            );

            // Seed some timeline entries
            modelBuilder.Entity<TimelineEntry>().HasData(
                new TimelineEntry
                {
                    Id = 1,
                    UserId = 1,
                    Title = "Started Learning Web Development",
                    Description = "Began my journey into full-stack web development with .NET and React",
                    EventDate = new DateTime(2024, 1, 15),
                    EntryType = "Milestone",
                    Category = "Education",
                    SourceApi = "Manual",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new TimelineEntry
                {
                    Id = 2,
                    UserId = 1,
                    Title = "Completed First React Project",
                    Description = "Built a personal portfolio website using React and TypeScript",
                    EventDate = new DateTime(2024, 3, 22),
                    EntryType = "Achievement",
                    Category = "Projects",
                    SourceApi = "Manual",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new TimelineEntry
                {
                    Id = 3,
                    UserId = 1,
                    Title = "First GitHub Contribution",
                    Description = "Made my first open source contribution to a popular repository",
                    EventDate = new DateTime(2024, 5, 10),
                    EntryType = "Achievement",
                    Category = "Open Source",
                    SourceApi = "Manual",
                    ExternalUrl = "https://github.com",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new TimelineEntry
                {
                    Id = 4,
                    UserId = 1,
                    Title = "Started Fitness Journey",
                    Description = "Committed to a healthier lifestyle with daily exercise",
                    EventDate = new DateTime(2024, 6, 1),
                    EntryType = "Milestone",
                    Category = "Health",
                    SourceApi = "Manual",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new TimelineEntry
                {
                    Id = 5,
                    UserId = 1,
                    Title = "Discovered New Music Genre",
                    Description = "Fell in love with indie folk music",
                    EventDate = new DateTime(2024, 7, 15),
                    EntryType = "Memory",
                    Category = "Music",
                    SourceApi = "Manual",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );
        }
    }
}