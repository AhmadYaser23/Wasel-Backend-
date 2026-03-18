using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectWasel.Migrations
{
    /// <inheritdoc />
    public partial class @in : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CheckpointStatusHistories",
                keyColumn: "HistoryId",
                keyValue: 1,
                column: "ChangedAt",
                value: new DateTime(2026, 3, 13, 19, 42, 15, 476, DateTimeKind.Utc).AddTicks(8282));

            migrationBuilder.UpdateData(
                table: "CheckpointStatusHistories",
                keyColumn: "HistoryId",
                keyValue: 2,
                column: "ChangedAt",
                value: new DateTime(2026, 3, 13, 19, 42, 15, 476, DateTimeKind.Utc).AddTicks(8282));

            migrationBuilder.UpdateData(
                table: "Checkpoints",
                keyColumn: "CheckpointId",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 13, 19, 42, 15, 476, DateTimeKind.Utc).AddTicks(8282));

            migrationBuilder.UpdateData(
                table: "Checkpoints",
                keyColumn: "CheckpointId",
                keyValue: 2,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 13, 19, 42, 15, 476, DateTimeKind.Utc).AddTicks(8282));

            migrationBuilder.UpdateData(
                table: "ExternalData",
                keyColumn: "DataId",
                keyValue: 1,
                column: "FetchedAt",
                value: new DateTime(2026, 3, 13, 19, 42, 15, 476, DateTimeKind.Utc).AddTicks(8282));

            migrationBuilder.UpdateData(
                table: "ExternalData",
                keyColumn: "DataId",
                keyValue: 2,
                column: "FetchedAt",
                value: new DateTime(2026, 3, 13, 19, 42, 15, 476, DateTimeKind.Utc).AddTicks(8282));

            migrationBuilder.UpdateData(
                table: "Incidents",
                keyColumn: "IncidentId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 13, 19, 42, 15, 476, DateTimeKind.Utc).AddTicks(8282), new DateTime(2026, 3, 13, 19, 42, 15, 476, DateTimeKind.Utc).AddTicks(8282) });

            migrationBuilder.UpdateData(
                table: "Incidents",
                keyColumn: "IncidentId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 13, 19, 42, 15, 476, DateTimeKind.Utc).AddTicks(8282), new DateTime(2026, 3, 13, 19, 42, 15, 476, DateTimeKind.Utc).AddTicks(8282) });

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "ReportId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 13, 19, 42, 15, 476, DateTimeKind.Utc).AddTicks(8282));

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "ReportId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 13, 19, 42, 15, 476, DateTimeKind.Utc).AddTicks(8282));

            migrationBuilder.UpdateData(
                table: "Routes",
                keyColumn: "RouteId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 13, 19, 42, 15, 476, DateTimeKind.Utc).AddTicks(8282));

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 13, 19, 42, 15, 476, DateTimeKind.Utc).AddTicks(8282));

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 13, 19, 42, 15, 476, DateTimeKind.Utc).AddTicks(8282));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 13, 19, 42, 15, 476, DateTimeKind.Utc).AddTicks(8282));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 13, 19, 42, 15, 476, DateTimeKind.Utc).AddTicks(8282));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 13, 19, 42, 15, 476, DateTimeKind.Utc).AddTicks(8282));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CheckpointStatusHistories",
                keyColumn: "HistoryId",
                keyValue: 1,
                column: "ChangedAt",
                value: new DateTime(2026, 3, 10, 18, 32, 43, 146, DateTimeKind.Utc).AddTicks(413));

            migrationBuilder.UpdateData(
                table: "CheckpointStatusHistories",
                keyColumn: "HistoryId",
                keyValue: 2,
                column: "ChangedAt",
                value: new DateTime(2026, 3, 10, 18, 32, 43, 146, DateTimeKind.Utc).AddTicks(413));

            migrationBuilder.UpdateData(
                table: "Checkpoints",
                keyColumn: "CheckpointId",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 10, 18, 32, 43, 146, DateTimeKind.Utc).AddTicks(413));

            migrationBuilder.UpdateData(
                table: "Checkpoints",
                keyColumn: "CheckpointId",
                keyValue: 2,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 10, 18, 32, 43, 146, DateTimeKind.Utc).AddTicks(413));

            migrationBuilder.UpdateData(
                table: "ExternalData",
                keyColumn: "DataId",
                keyValue: 1,
                column: "FetchedAt",
                value: new DateTime(2026, 3, 10, 18, 32, 43, 146, DateTimeKind.Utc).AddTicks(413));

            migrationBuilder.UpdateData(
                table: "ExternalData",
                keyColumn: "DataId",
                keyValue: 2,
                column: "FetchedAt",
                value: new DateTime(2026, 3, 10, 18, 32, 43, 146, DateTimeKind.Utc).AddTicks(413));

            migrationBuilder.UpdateData(
                table: "Incidents",
                keyColumn: "IncidentId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 10, 18, 32, 43, 146, DateTimeKind.Utc).AddTicks(413), new DateTime(2026, 3, 10, 18, 32, 43, 146, DateTimeKind.Utc).AddTicks(413) });

            migrationBuilder.UpdateData(
                table: "Incidents",
                keyColumn: "IncidentId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 10, 18, 32, 43, 146, DateTimeKind.Utc).AddTicks(413), new DateTime(2026, 3, 10, 18, 32, 43, 146, DateTimeKind.Utc).AddTicks(413) });

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "ReportId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 10, 18, 32, 43, 146, DateTimeKind.Utc).AddTicks(413));

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "ReportId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 10, 18, 32, 43, 146, DateTimeKind.Utc).AddTicks(413));

            migrationBuilder.UpdateData(
                table: "Routes",
                keyColumn: "RouteId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 10, 18, 32, 43, 146, DateTimeKind.Utc).AddTicks(413));

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 10, 18, 32, 43, 146, DateTimeKind.Utc).AddTicks(413));

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 10, 18, 32, 43, 146, DateTimeKind.Utc).AddTicks(413));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 10, 18, 32, 43, 146, DateTimeKind.Utc).AddTicks(413));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 10, 18, 32, 43, 146, DateTimeKind.Utc).AddTicks(413));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 10, 18, 32, 43, 146, DateTimeKind.Utc).AddTicks(413));
        }
    }
}
