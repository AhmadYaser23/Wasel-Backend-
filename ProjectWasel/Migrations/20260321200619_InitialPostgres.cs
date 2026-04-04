using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProjectWasel.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Checkpoints",
                columns: table => new
                {
                    CheckpointId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Latitude = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: false),
                    Longitude = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Checkpoints", x => x.CheckpointId);
                });

            migrationBuilder.CreateTable(
                name: "ExternalData",
                columns: table => new
                {
                    DataId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Source = table.Column<string>(type: "text", nullable: false),
                    ExternalKey = table.Column<string>(type: "text", nullable: false),
                    JsonData = table.Column<string>(type: "text", nullable: false),
                    FetchedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalData", x => x.DataId);
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    RouteId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StartLat = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: false),
                    StartLng = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: false),
                    EndLat = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: false),
                    EndLng = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: false),
                    EstimatedDistance = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    EstimatedDuration = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.RouteId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "CheckpointStatusHistories",
                columns: table => new
                {
                    HistoryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CheckpointId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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
                    IncidentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Severity = table.Column<string>(type: "text", nullable: false),
                    CheckpointId = table.Column<int>(type: "integer", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "integer", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: true)
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
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Token = table.Column<string>(type: "text", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
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
                name: "Subscriptions",
                columns: table => new
                {
                    SubscriptionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    GeographicArea = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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
                    ReportId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    IncidentId = table.Column<int>(type: "integer", nullable: true),
                    Latitude = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: false),
                    Longitude = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Votes = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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
                    { 1, new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930), 31.9539m, 35.2061m, "Qalandia", "active" },
                    { 2, new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930), 31.7054m, 35.2024m, "Bethlehem 300", "delayed" }
                });

            migrationBuilder.InsertData(
                table: "ExternalData",
                columns: new[] { "DataId", "ExternalKey", "FetchedAt", "JsonData", "Source" },
                values: new object[,]
                {
                    { 1, "weather_1", new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930), "{\"temp\":25,\"status\":\"sunny\"}", "WeatherAPI" },
                    { 2, "map_1", new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930), "{\"road\":\"open\"}", "OpenStreetMap" }
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "RouteId", "CreatedAt", "EndLat", "EndLng", "EstimatedDistance", "EstimatedDuration", "StartLat", "StartLng" },
                values: new object[] { 1, new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930), 31.7054m, 35.2024m, 30.5m, 0.76m, 31.9539m, 35.2061m });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930), "ahmed@wasel.ps", "SECRET_HASH_ABC", "admin", "Ahmed_Admin" },
                    { 2, new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930), "noor@wasel.ps", "SECRET_HASH_XYZ", "moderator", "Noor_Mod" },
                    { 3, new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930), "mohammad@wasel.ps", "SECRET_HASH_123", "citizen", "Mohammad_User" }
                });

            migrationBuilder.InsertData(
                table: "CheckpointStatusHistories",
                columns: new[] { "HistoryId", "ChangedAt", "CheckpointId", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930), 1, "active" },
                    { 2, new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930), 2, "delayed" }
                });

            migrationBuilder.InsertData(
                table: "Incidents",
                columns: new[] { "IncidentId", "CheckpointId", "CreatedAt", "CreatedByUserId", "Description", "Severity", "Status", "Title", "Type", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930), 3, "Minor accident", "low", "verified", "Accident at Qalandia", "accident", new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930), null },
                    { 2, 2, new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930), 3, "Temporary closure", "medium", "verified", "Closure at Bethlehem 300", "closure", new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930), null }
                });

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "Category", "CreatedAt", "GeographicArea", "UserId" },
                values: new object[,]
                {
                    { 1, "Accident", new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930), "Qalandia", 3 },
                    { 2, "Closure", new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930), "Bethlehem", 2 }
                });

            migrationBuilder.InsertData(
                table: "Reports",
                columns: new[] { "ReportId", "Category", "CreatedAt", "Description", "IncidentId", "Latitude", "Longitude", "UserId", "Votes" },
                values: new object[,]
                {
                    { 1, "Accident", new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930), "Saw minor accident", 1, 31.9539m, 35.2061m, 3, 5 },
                    { 2, "Closure", new DateTime(2026, 3, 21, 20, 6, 18, 743, DateTimeKind.Utc).AddTicks(9930), "Road closed temporarily", 2, 31.7054m, 35.2024m, 2, 3 }
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
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
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
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "Routes");

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
