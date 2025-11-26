using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PersonalTimelineAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OAuthProvider = table.Column<string>(type: "TEXT", nullable: false),
                    OAuthId = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: false),
                    ProfileImageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApiConnections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    ApiProvider = table.Column<string>(type: "TEXT", nullable: false),
                    AccessToken = table.Column<string>(type: "TEXT", nullable: false),
                    RefreshToken = table.Column<string>(type: "TEXT", nullable: false),
                    TokenExpiresAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastSyncAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Settings = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiConnections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApiConnections_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimelineEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    EventDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EntryType = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    ExternalUrl = table.Column<string>(type: "TEXT", nullable: false),
                    SourceApi = table.Column<string>(type: "TEXT", nullable: false),
                    ExternalId = table.Column<string>(type: "TEXT", nullable: false),
                    Metadata = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimelineEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimelineEntries_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "DisplayName", "Email", "LastLoginAt", "OAuthId", "OAuthProvider", "ProfileImageUrl" },
                values: new object[] { 1, new DateTime(2025, 11, 11, 18, 48, 44, 615, DateTimeKind.Utc).AddTicks(7760), "Test User", "testuser@example.com", new DateTime(2025, 11, 11, 18, 48, 44, 615, DateTimeKind.Utc).AddTicks(7760), "test-oauth-id-123", "Google", "https://via.placeholder.com/150" });

            migrationBuilder.InsertData(
                table: "TimelineEntries",
                columns: new[] { "Id", "Category", "CreatedAt", "Description", "EntryType", "EventDate", "ExternalId", "ExternalUrl", "ImageUrl", "Metadata", "SourceApi", "Title", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 1, "Education", new DateTime(2025, 11, 11, 18, 48, 44, 615, DateTimeKind.Utc).AddTicks(7810), "Began my journey into full-stack web development with .NET and React", "Milestone", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "", "", "{}", "Manual", "Started Learning Web Development", new DateTime(2025, 11, 11, 18, 48, 44, 615, DateTimeKind.Utc).AddTicks(7810), 1 },
                    { 2, "Projects", new DateTime(2025, 11, 11, 18, 48, 44, 615, DateTimeKind.Utc).AddTicks(7810), "Built a personal portfolio website using React and TypeScript", "Achievement", new DateTime(2024, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "", "", "{}", "Manual", "Completed First React Project", new DateTime(2025, 11, 11, 18, 48, 44, 615, DateTimeKind.Utc).AddTicks(7810), 1 },
                    { 3, "Open Source", new DateTime(2025, 11, 11, 18, 48, 44, 615, DateTimeKind.Utc).AddTicks(7820), "Made my first open source contribution to a popular repository", "Achievement", new DateTime(2024, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "https://github.com", "", "{}", "Manual", "First GitHub Contribution", new DateTime(2025, 11, 11, 18, 48, 44, 615, DateTimeKind.Utc).AddTicks(7820), 1 },
                    { 4, "Health", new DateTime(2025, 11, 11, 18, 48, 44, 615, DateTimeKind.Utc).AddTicks(7820), "Committed to a healthier lifestyle with daily exercise", "Milestone", new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "", "", "{}", "Manual", "Started Fitness Journey", new DateTime(2025, 11, 11, 18, 48, 44, 615, DateTimeKind.Utc).AddTicks(7820), 1 },
                    { 5, "Music", new DateTime(2025, 11, 11, 18, 48, 44, 615, DateTimeKind.Utc).AddTicks(7820), "Fell in love with indie folk music", "Memory", new DateTime(2024, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "", "", "{}", "Manual", "Discovered New Music Genre", new DateTime(2025, 11, 11, 18, 48, 44, 615, DateTimeKind.Utc).AddTicks(7820), 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiConnections_UserId",
                table: "ApiConnections",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TimelineEntries_UserId",
                table: "TimelineEntries",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiConnections");

            migrationBuilder.DropTable(
                name: "TimelineEntries");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
