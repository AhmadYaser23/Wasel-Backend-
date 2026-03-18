using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProjectWasel.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Checkpoints",
                columns: table => new
                {
                    CheckpointId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Checkpoints", x => x.CheckpointId);
                });

            migrationBuilder.CreateTable(
                name: "ExternalData",
                columns: table => new
                {
                    DataId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExternalKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JsonData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FetchedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalData", x => x.DataId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "CheckpointStatusHistories",
                columns: table => new
                {
                    HistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CheckpointId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckpointStatusHistories", x => x.HistoryId);
                    table.ForeignKey(
                        name: "FK_CheckpointStatusHistories_Checkpoints_CheckpointId",
                        column: x => x.CheckpointId,
                        principalTable: "Checkpoints",
                        principalColumn: "CheckpointId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Incidents",
                columns: table => new
                {
                    IncidentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CheckpointId = table.Column<int>(type: "int", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incidents", x => x.IncidentId);
                    table.ForeignKey(
                        name: "FK_Incidents_Checkpoints_CheckpointId",
                        column: x => x.CheckpointId,
                        principalTable: "Checkpoints",
                        principalColumn: "CheckpointId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Incidents_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Incidents_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    SubscriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    GeographicArea = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.SubscriptionId);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    IncidentId = table.Column<int>(type: "int", nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Votes = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_Reports_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "IncidentId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Reports_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Checkpoints",
                columns: new[] { "CheckpointId", "LastUpdated", "Latitude", "Longitude", "Name", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 3, 1, 20, 32, 50, 451, DateTimeKind.Utc).AddTicks(5036), 31.9539m, 35.2061m, "Qalandia", "active" },
                    { 2, new DateTime(2026, 3, 1, 20, 32, 50, 451, DateTimeKind.Utc).AddTicks(5036), 31.7054m, 35.2024m, "Bethlehem 300", "delayed" }
                });

            migrationBuilder.InsertData(
                table: "ExternalData",
                columns: new[] { "DataId", "ExternalKey", "FetchedAt", "JsonData", "Source" },
                values: new object[,]
                {
                    { 1, "weather_1", new DateTime(2026, 3, 1, 20, 32, 50, 451, DateTimeKind.Utc).AddTicks(5036), "{\"temp\":25,\"status\":\"sunny\"}", "WeatherAPI" },
                    { 2, "map_1", new DateTime(2026, 3, 1, 20, 32, 50, 451, DateTimeKind.Utc).AddTicks(5036), "{\"road\":\"open\"}", "OpenStreetMap" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 3, 1, 20, 32, 50, 451, DateTimeKind.Utc).AddTicks(5036), "ahmed@wasel.ps", "SECRET_HASH_ABC", "admin", "Ahmed_Admin" },
                    { 2, new DateTime(2026, 3, 1, 20, 32, 50, 451, DateTimeKind.Utc).AddTicks(5036), "noor@wasel.ps", "SECRET_HASH_XYZ", "moderator", "Noor_Mod" },
                    { 3, new DateTime(2026, 3, 1, 20, 32, 50, 451, DateTimeKind.Utc).AddTicks(5036), "mohammad@wasel.ps", "SECRET_HASH_123", "citizen", "Mohammad_User" }
                });

            migrationBuilder.InsertData(
                table: "CheckpointStatusHistories",
                columns: new[] { "HistoryId", "ChangedAt", "CheckpointId", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 3, 1, 20, 32, 50, 451, DateTimeKind.Utc).AddTicks(5036), 1, "active" },
                    { 2, new DateTime(2026, 3, 1, 20, 32, 50, 451, DateTimeKind.Utc).AddTicks(5036), 2, "delayed" }
                });

            migrationBuilder.InsertData(
                table: "Incidents",
                columns: new[] { "IncidentId", "CheckpointId", "CreatedAt", "CreatedByUserId", "Description", "Severity", "Status", "Title", "Type", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 3, 1, 20, 32, 50, 451, DateTimeKind.Utc).AddTicks(5036), 3, "Minor accident", "low", "verified", "Accident at Qalandia", "accident", new DateTime(2026, 3, 1, 20, 32, 50, 451, DateTimeKind.Utc).AddTicks(5036), null },
                    { 2, 2, new DateTime(2026, 3, 1, 20, 32, 50, 451, DateTimeKind.Utc).AddTicks(5036), 3, "Temporary closure", "medium", "verified", "Closure at Bethlehem 300", "closure", new DateTime(2026, 3, 1, 20, 32, 50, 451, DateTimeKind.Utc).AddTicks(5036), null }
                });

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "Category", "CreatedAt", "GeographicArea", "UserId" },
                values: new object[,]
                {
                    { 1, "Accident", new DateTime(2026, 3, 1, 20, 32, 50, 451, DateTimeKind.Utc).AddTicks(5036), "Qalandia", 3 },
                    { 2, "Closure", new DateTime(2026, 3, 1, 20, 32, 50, 451, DateTimeKind.Utc).AddTicks(5036), "Bethlehem", 2 }
                });

            migrationBuilder.InsertData(
                table: "Reports",
                columns: new[] { "ReportId", "Category", "CreatedAt", "Description", "IncidentId", "Latitude", "Longitude", "UserId", "Votes" },
                values: new object[,]
                {
                    { 1, "Accident", new DateTime(2026, 3, 1, 20, 32, 50, 451, DateTimeKind.Utc).AddTicks(5036), "Saw minor accident", 1, 31.9539m, 35.2061m, 3, 5 },
                    { 2, "Closure", new DateTime(2026, 3, 1, 20, 32, 50, 451, DateTimeKind.Utc).AddTicks(5036), "Road closed temporarily", 2, 31.7054m, 35.2024m, 2, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckpointStatusHistories_CheckpointId",
                table: "CheckpointStatusHistories",
                column: "CheckpointId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_CheckpointId",
                table: "Incidents",
                column: "CheckpointId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_CreatedByUserId",
                table: "Incidents",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_UserId",
                table: "Incidents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_IncidentId",
                table: "Reports",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_UserId",
                table: "Reports",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_UserId",
                table: "Subscriptions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckpointStatusHistories");

            migrationBuilder.DropTable(
                name: "ExternalData");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Incidents");

            migrationBuilder.DropTable(
                name: "Checkpoints");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
