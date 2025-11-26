using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalTimelineAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddGitHubIntegration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GitHubIntegrations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    GitHubUsername = table.Column<string>(type: "TEXT", nullable: false),
                    AccessToken = table.Column<string>(type: "TEXT", nullable: false),
                    ConnectedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastSyncedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GitHubIntegrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GitHubIntegrations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "TimelineEntries",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 24, 2, 24, 40, 878, DateTimeKind.Utc).AddTicks(610), new DateTime(2025, 11, 24, 2, 24, 40, 878, DateTimeKind.Utc).AddTicks(610) });

            migrationBuilder.UpdateData(
                table: "TimelineEntries",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 24, 2, 24, 40, 878, DateTimeKind.Utc).AddTicks(610), new DateTime(2025, 11, 24, 2, 24, 40, 878, DateTimeKind.Utc).AddTicks(610) });

            migrationBuilder.UpdateData(
                table: "TimelineEntries",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 24, 2, 24, 40, 878, DateTimeKind.Utc).AddTicks(610), new DateTime(2025, 11, 24, 2, 24, 40, 878, DateTimeKind.Utc).AddTicks(610) });

            migrationBuilder.UpdateData(
                table: "TimelineEntries",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 24, 2, 24, 40, 878, DateTimeKind.Utc).AddTicks(610), new DateTime(2025, 11, 24, 2, 24, 40, 878, DateTimeKind.Utc).AddTicks(610) });

            migrationBuilder.UpdateData(
                table: "TimelineEntries",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 24, 2, 24, 40, 878, DateTimeKind.Utc).AddTicks(610), new DateTime(2025, 11, 24, 2, 24, 40, 878, DateTimeKind.Utc).AddTicks(610) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastLoginAt" },
                values: new object[] { new DateTime(2025, 11, 24, 2, 24, 40, 878, DateTimeKind.Utc).AddTicks(560), new DateTime(2025, 11, 24, 2, 24, 40, 878, DateTimeKind.Utc).AddTicks(560) });

            migrationBuilder.CreateIndex(
                name: "IX_GitHubIntegrations_UserId",
                table: "GitHubIntegrations",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GitHubIntegrations");

            migrationBuilder.UpdateData(
                table: "TimelineEntries",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 22, 17, 44, 44, 360, DateTimeKind.Utc).AddTicks(9560), new DateTime(2025, 11, 22, 17, 44, 44, 360, DateTimeKind.Utc).AddTicks(9560) });

            migrationBuilder.UpdateData(
                table: "TimelineEntries",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 22, 17, 44, 44, 360, DateTimeKind.Utc).AddTicks(9560), new DateTime(2025, 11, 22, 17, 44, 44, 360, DateTimeKind.Utc).AddTicks(9560) });

            migrationBuilder.UpdateData(
                table: "TimelineEntries",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 22, 17, 44, 44, 360, DateTimeKind.Utc).AddTicks(9560), new DateTime(2025, 11, 22, 17, 44, 44, 360, DateTimeKind.Utc).AddTicks(9560) });

            migrationBuilder.UpdateData(
                table: "TimelineEntries",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 22, 17, 44, 44, 360, DateTimeKind.Utc).AddTicks(9560), new DateTime(2025, 11, 22, 17, 44, 44, 360, DateTimeKind.Utc).AddTicks(9560) });

            migrationBuilder.UpdateData(
                table: "TimelineEntries",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 22, 17, 44, 44, 360, DateTimeKind.Utc).AddTicks(9570), new DateTime(2025, 11, 22, 17, 44, 44, 360, DateTimeKind.Utc).AddTicks(9570) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastLoginAt" },
                values: new object[] { new DateTime(2025, 11, 22, 17, 44, 44, 360, DateTimeKind.Utc).AddTicks(9500), new DateTime(2025, 11, 22, 17, 44, 44, 360, DateTimeKind.Utc).AddTicks(9500) });
        }
    }
}
