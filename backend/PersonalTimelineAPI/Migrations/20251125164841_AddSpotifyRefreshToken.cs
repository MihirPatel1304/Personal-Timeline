using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalTimelineAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddSpotifyRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "SpotifyIntegrations",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "TokenExpiresAt",
                table: "SpotifyIntegrations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "TimelineEntries",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 25, 16, 48, 41, 818, DateTimeKind.Utc).AddTicks(3910), new DateTime(2025, 11, 25, 16, 48, 41, 818, DateTimeKind.Utc).AddTicks(3920) });

            migrationBuilder.UpdateData(
                table: "TimelineEntries",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 25, 16, 48, 41, 818, DateTimeKind.Utc).AddTicks(3920), new DateTime(2025, 11, 25, 16, 48, 41, 818, DateTimeKind.Utc).AddTicks(3920) });

            migrationBuilder.UpdateData(
                table: "TimelineEntries",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 25, 16, 48, 41, 818, DateTimeKind.Utc).AddTicks(3920), new DateTime(2025, 11, 25, 16, 48, 41, 818, DateTimeKind.Utc).AddTicks(3920) });

            migrationBuilder.UpdateData(
                table: "TimelineEntries",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 25, 16, 48, 41, 818, DateTimeKind.Utc).AddTicks(3920), new DateTime(2025, 11, 25, 16, 48, 41, 818, DateTimeKind.Utc).AddTicks(3920) });

            migrationBuilder.UpdateData(
                table: "TimelineEntries",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 25, 16, 48, 41, 818, DateTimeKind.Utc).AddTicks(3920), new DateTime(2025, 11, 25, 16, 48, 41, 818, DateTimeKind.Utc).AddTicks(3920) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastLoginAt" },
                values: new object[] { new DateTime(2025, 11, 25, 16, 48, 41, 818, DateTimeKind.Utc).AddTicks(3860), new DateTime(2025, 11, 25, 16, 48, 41, 818, DateTimeKind.Utc).AddTicks(3860) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "SpotifyIntegrations");

            migrationBuilder.DropColumn(
                name: "TokenExpiresAt",
                table: "SpotifyIntegrations");

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
    }
}
