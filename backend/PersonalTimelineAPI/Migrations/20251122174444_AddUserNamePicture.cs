using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalTimelineAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddUserNamePicture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

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
                columns: new[] { "CreatedAt", "LastLoginAt", "Name", "Picture" },
                values: new object[] { new DateTime(2025, 11, 22, 17, 44, 44, 360, DateTimeKind.Utc).AddTicks(9500), new DateTime(2025, 11, 22, 17, 44, 44, 360, DateTimeKind.Utc).AddTicks(9500), "", "" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "TimelineEntries",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 11, 18, 48, 44, 615, DateTimeKind.Utc).AddTicks(7810), new DateTime(2025, 11, 11, 18, 48, 44, 615, DateTimeKind.Utc).AddTicks(7810) });

            migrationBuilder.UpdateData(
                table: "TimelineEntries",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 11, 18, 48, 44, 615, DateTimeKind.Utc).AddTicks(7810), new DateTime(2025, 11, 11, 18, 48, 44, 615, DateTimeKind.Utc).AddTicks(7810) });

            migrationBuilder.UpdateData(
                table: "TimelineEntries",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 11, 18, 48, 44, 615, DateTimeKind.Utc).AddTicks(7820), new DateTime(2025, 11, 11, 18, 48, 44, 615, DateTimeKind.Utc).AddTicks(7820) });

            migrationBuilder.UpdateData(
                table: "TimelineEntries",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 11, 18, 48, 44, 615, DateTimeKind.Utc).AddTicks(7820), new DateTime(2025, 11, 11, 18, 48, 44, 615, DateTimeKind.Utc).AddTicks(7820) });

            migrationBuilder.UpdateData(
                table: "TimelineEntries",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 11, 18, 48, 44, 615, DateTimeKind.Utc).AddTicks(7820), new DateTime(2025, 11, 11, 18, 48, 44, 615, DateTimeKind.Utc).AddTicks(7820) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastLoginAt" },
                values: new object[] { new DateTime(2025, 11, 11, 18, 48, 44, 615, DateTimeKind.Utc).AddTicks(7760), new DateTime(2025, 11, 11, 18, 48, 44, 615, DateTimeKind.Utc).AddTicks(7760) });
        }
    }
}
