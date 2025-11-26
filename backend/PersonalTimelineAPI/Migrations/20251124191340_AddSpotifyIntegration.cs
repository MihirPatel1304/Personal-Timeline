using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalTimelineAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddSpotifyIntegration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpotifyIntegrations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    AccessToken = table.Column<string>(type: "TEXT", nullable: false),
                    ConnectedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastSyncedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpotifyIntegrations", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "TimelineEntries",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 24, 19, 13, 39, 998, DateTimeKind.Utc).AddTicks(7280), new DateTime(2025, 11, 24, 19, 13, 39, 998, DateTimeKind.Utc).AddTicks(7280) });

            migrationBuilder.UpdateData(
                table: "TimelineEntries",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 24, 19, 13, 39, 998, DateTimeKind.Utc).AddTicks(7280), new DateTime(2025, 11, 24, 19, 13, 39, 998, DateTimeKind.Utc).AddTicks(7280) });

            migrationBuilder.UpdateData(
                table: "TimelineEntries",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 24, 19, 13, 39, 998, DateTimeKind.Utc).AddTicks(7280), new DateTime(2025, 11, 24, 19, 13, 39, 998, DateTimeKind.Utc).AddTicks(7280) });

            migrationBuilder.UpdateData(
                table: "TimelineEntries",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 24, 19, 13, 39, 998, DateTimeKind.Utc).AddTicks(7280), new DateTime(2025, 11, 24, 19, 13, 39, 998, DateTimeKind.Utc).AddTicks(7280) });

            migrationBuilder.UpdateData(
                table: "TimelineEntries",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 24, 19, 13, 39, 998, DateTimeKind.Utc).AddTicks(7280), new DateTime(2025, 11, 24, 19, 13, 39, 998, DateTimeKind.Utc).AddTicks(7280) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastLoginAt" },
                values: new object[] { new DateTime(2025, 11, 24, 19, 13, 39, 998, DateTimeKind.Utc).AddTicks(7230), new DateTime(2025, 11, 24, 19, 13, 39, 998, DateTimeKind.Utc).AddTicks(7230) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpotifyIntegrations");

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
        }
    }
}
