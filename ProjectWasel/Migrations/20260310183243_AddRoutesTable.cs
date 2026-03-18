using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectWasel.Migrations
{
    /// <inheritdoc />
    public partial class AddRoutesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    RouteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartLat = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    StartLng = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    EndLat = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    EndLng = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    EstimatedDistance = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    EstimatedDuration = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.RouteId);
                });

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

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "RouteId", "CreatedAt", "EndLat", "EndLng", "EstimatedDistance", "EstimatedDuration", "StartLat", "StartLng" },
                values: new object[] { 1, new DateTime(2026, 3, 10, 18, 32, 43, 146, DateTimeKind.Utc).AddTicks(413), 31.7054m, 35.2024m, 30.5m, 0.76m, 31.9539m, 35.2061m });

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

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.UpdateData(
                table: "CheckpointStatusHistories",
                keyColumn: "HistoryId",
                keyValue: 1,
                column: "ChangedAt",
                value: new DateTime(2026, 3, 1, 20, 32, 50, 451, DateTimeKind.Utc).AddTicks(5036));

            migrationBuilder.UpdateData(
                table: "CheckpointStatusHistories",
                keyColumn: "HistoryId",
                keyValue: 2,
                column: "ChangedAt",
                value: new DateTime(2026, 3, 1, 20, 32, 50, 451, DateTimeKind.Utc).AddTicks(5036));

            migrationBuilder.UpdateData(
                table: "Checkpoints",
                keyColumn: "CheckpointId",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 1, 20, 32, 50, 451, DateTimeKind.Utc).AddTicks(5036));

            migrationBuilder.UpdateData(
                table: "Checkpoints",
                keyColumn: "CheckpointId",
                keyValue: 2,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 1, 20, 32, 50, 451, DateTimeKind.Utc).AddTicks(5036));

            migrationBuilder.UpdateData(
                table: "ExternalData",
                keyColumn: "DataId",
                keyValue: 1,
                column: "FetchedAt",
                value: new DateTime(2026, 3, 1, 20, 32, 50, 451, DateTimeKind.Utc).AddTicks(5036));

            migrationBuilder.UpdateData(
                table: "ExternalData",
                keyColumn: "DataId",
                keyValue: 2,
                column: "FetchedAt",
                value: new DateTime(2026, 3, 1, 20, 32, 50, 451, DateTimeKind.Utc).AddTicks(5036));

            migrationBuilder.UpdateData(
                table: "Incidents",
                keyColumn: "IncidentId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 1, 20, 32, 50, 451, DateTimeKind.Utc).AddTicks(5036), new DateTime(2026, 3, 1, 20, 32, 50, 451, DateTimeKind.Utc).AddTicks(5036) });

            migrationBuilder.UpdateData(
                table: "Incidents",
                keyColumn: "IncidentId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 1, 20, 32, 50, 451, DateTimeKind.Utc).AddTicks(5036), new DateTime(2026, 3, 1, 20, 32, 50, 451, DateTimeKind.Utc).AddTicks(5036) });

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "ReportId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 1, 20, 32, 50, 451, DateTimeKind.Utc).AddTicks(5036));

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "ReportId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 1, 20, 32, 50, 451, DateTimeKind.Utc).AddTicks(5036));

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 1, 20, 32, 50, 451, DateTimeKind.Utc).AddTicks(5036));

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 1, 20, 32, 50, 451, DateTimeKind.Utc).AddTicks(5036));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 1, 20, 32, 50, 451, DateTimeKind.Utc).AddTicks(5036));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 1, 20, 32, 50, 451, DateTimeKind.Utc).AddTicks(5036));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 1, 20, 32, 50, 451, DateTimeKind.Utc).AddTicks(5036));
        }
    }
}
