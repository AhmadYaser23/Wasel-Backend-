using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectWasel.Migrations
{
    /// <inheritdoc />
    public partial class FixPostgresSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CheckpointStatusHistories",
                keyColumn: "HistoryId",
                keyValue: 1,
                column: "ChangedAt",
                value: new DateTime(2026, 3, 21, 20, 32, 51, 90, DateTimeKind.Utc).AddTicks(8760));

            migrationBuilder.UpdateData(
                table: "CheckpointStatusHistories",
                keyColumn: "HistoryId",
                keyValue: 2,
                column: "ChangedAt",
                value: new DateTime(2026, 3, 21, 20, 32, 51, 90, DateTimeKind.Utc).AddTicks(8760));

            migrationBuilder.UpdateData(
                table: "Checkpoints",
                keyColumn: "CheckpointId",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 21, 20, 32, 51, 90, DateTimeKind.Utc).AddTicks(8760));

            migrationBuilder.UpdateData(
                table: "Checkpoints",
                keyColumn: "CheckpointId",
                keyValue: 2,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 21, 20, 32, 51, 90, DateTimeKind.Utc).AddTicks(8760));

            migrationBuilder.UpdateData(
                table: "ExternalData",
                keyColumn: "DataId",
                keyValue: 1,
                column: "FetchedAt",
                value: new DateTime(2026, 3, 21, 20, 32, 51, 90, DateTimeKind.Utc).AddTicks(8760));

            migrationBuilder.UpdateData(
                table: "ExternalData",
                keyColumn: "DataId",
                keyValue: 2,
                column: "FetchedAt",
                value: new DateTime(2026, 3, 21, 20, 32, 51, 90, DateTimeKind.Utc).AddTicks(8760));

            migrationBuilder.UpdateData(
                table: "Incidents",
                keyColumn: "IncidentId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 21, 20, 32, 51, 90, DateTimeKind.Utc).AddTicks(8760), new DateTime(2026, 3, 21, 20, 32, 51, 90, DateTimeKind.Utc).AddTicks(8760) });

            migrationBuilder.UpdateData(
                table: "Incidents",
                keyColumn: "IncidentId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 21, 20, 32, 51, 90, DateTimeKind.Utc).AddTicks(8760), new DateTime(2026, 3, 21, 20, 32, 51, 90, DateTimeKind.Utc).AddTicks(8760) });

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "ReportId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 21, 20, 32, 51, 90, DateTimeKind.Utc).AddTicks(8760));

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "ReportId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 21, 20, 32, 51, 90, DateTimeKind.Utc).AddTicks(8760));

            migrationBuilder.UpdateData(
                table: "Routes",
                keyColumn: "RouteId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 21, 20, 32, 51, 90, DateTimeKind.Utc).AddTicks(8760));

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 21, 20, 32, 51, 90, DateTimeKind.Utc).AddTicks(8760));

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 21, 20, 32, 51, 90, DateTimeKind.Utc).AddTicks(8760));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 21, 20, 32, 51, 90, DateTimeKind.Utc).AddTicks(8760));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 21, 20, 32, 51, 90, DateTimeKind.Utc).AddTicks(8760));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 21, 20, 32, 51, 90, DateTimeKind.Utc).AddTicks(8760));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CheckpointStatusHistories",
                keyColumn: "HistoryId",
                keyValue: 1,
                column: "ChangedAt",
                value: new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930));

            migrationBuilder.UpdateData(
                table: "CheckpointStatusHistories",
                keyColumn: "HistoryId",
                keyValue: 2,
                column: "ChangedAt",
                value: new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930));

            migrationBuilder.UpdateData(
                table: "Checkpoints",
                keyColumn: "CheckpointId",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930));

            migrationBuilder.UpdateData(
                table: "Checkpoints",
                keyColumn: "CheckpointId",
                keyValue: 2,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930));

            migrationBuilder.UpdateData(
                table: "ExternalData",
                keyColumn: "DataId",
                keyValue: 1,
                column: "FetchedAt",
                value: new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930));

            migrationBuilder.UpdateData(
                table: "ExternalData",
                keyColumn: "DataId",
                keyValue: 2,
                column: "FetchedAt",
                value: new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930));

            migrationBuilder.UpdateData(
                table: "Incidents",
                keyColumn: "IncidentId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930), new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930) });

            migrationBuilder.UpdateData(
                table: "Incidents",
                keyColumn: "IncidentId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930), new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930) });

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "ReportId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930));

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "ReportId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930));

            migrationBuilder.UpdateData(
                table: "Routes",
                keyColumn: "RouteId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930));

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930));

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930));
        }
    }
}
