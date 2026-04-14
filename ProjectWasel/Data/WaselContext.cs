using Microsoft.EntityFrameworkCore;
using ProjectWasel.Models;
using System;
using Route = ProjectWasel.Models.Route;

namespace ProjectWasel.Data
{
    public class WaselContext : DbContext
    {
        public WaselContext(DbContextOptions<WaselContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Checkpoint> Checkpoints { get; set; }
        public DbSet<CheckpointStatusHistory> CheckpointStatusHistories { get; set; }
        public DbSet<Incident> Incidents { get; set; }

        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportVote> ReportVotes { get; set; }
        public DbSet<ReportAuditLog> ReportAuditLogs { get; set; }

        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<ExternalData> ExternalData { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Route> Routes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var now = DateTime.UtcNow;

            // =========================
            // DECIMAL PRECISION
            // =========================
            modelBuilder.Entity<Report>().Property(x => x.Latitude).HasPrecision(9, 6);
            modelBuilder.Entity<Report>().Property(x => x.Longitude).HasPrecision(9, 6);

            modelBuilder.Entity<Route>().Property(x => x.StartLat).HasPrecision(9, 6);
            modelBuilder.Entity<Route>().Property(x => x.StartLng).HasPrecision(9, 6);
            modelBuilder.Entity<Route>().Property(x => x.EndLat).HasPrecision(9, 6);
            modelBuilder.Entity<Route>().Property(x => x.EndLng).HasPrecision(9, 6);
            modelBuilder.Entity<Route>().Property(x => x.EstimatedDistance).HasPrecision(10, 2);
            modelBuilder.Entity<Route>().Property(x => x.EstimatedDuration).HasPrecision(10, 2);

            // =========================
            // RELATIONS FIX
            // =========================
            modelBuilder.Entity<Incident>()
                .HasOne(i => i.CreatedByUser)
                .WithMany(u => u.CreatedIncidents)
                .HasForeignKey(i => i.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Incident>()
                .HasOne(i => i.Checkpoint)
                .WithMany(c => c.Incidents)
                .HasForeignKey(i => i.CheckpointId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reports)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.Incident)
                .WithMany(i => i.Reports)
                .HasForeignKey(r => r.IncidentId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ReportVote>()
                .HasOne(v => v.User)
                .WithMany(u => u.Votes)
                .HasForeignKey(v => v.UserId);

            modelBuilder.Entity<ReportVote>()
                .HasOne(v => v.Report)
                .WithMany()
                .HasForeignKey(v => v.ReportId);

            modelBuilder.Entity<ReportAuditLog>()
                .HasOne(a => a.Report)
                .WithMany()
                .HasForeignKey(a => a.ReportId);

            modelBuilder.Entity<ReportAuditLog>()
                .HasOne(a => a.Moderator)
                .WithMany()
                .HasForeignKey(a => a.ModeratorId);

            // =========================
            // USERS SEED
            // =========================
            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, Username = "Admin", Email = "admin@wasel.com", PasswordHash = "HASH1", Role = "admin", CreatedAt = now, IsActive = true },
                new User { UserId = 2, Username = "Moderator", Email = "mod@wasel.com", PasswordHash = "HASH2", Role = "moderator", CreatedAt = now, IsActive = true },
                new User { UserId = 3, Username = "User1", Email = "user1@wasel.com", PasswordHash = "HASH3", Role = "citizen", CreatedAt = now, IsActive = true }
            );

            // =========================
            // CHECKPOINTS SEED
            // =========================
            modelBuilder.Entity<Checkpoint>().HasData(
                new Checkpoint { CheckpointId = 1, Name = "Qalandia", Latitude = 31.9539m, Longitude = 35.2061m, Status = "active", LastUpdated = now },
                new Checkpoint { CheckpointId = 2, Name = "Bethlehem", Latitude = 31.7054m, Longitude = 35.2024m, Status = "active", LastUpdated = now }
            );

            // =========================
            // INCIDENTS SEED
            // =========================
            modelBuilder.Entity<Incident>().HasData(
                new Incident
                {
                    IncidentId = 1,
                    Title = "Accident",
                    Description = "Car accident",
                    Type = "accident",
                    Severity = "low",
                    CheckpointId = 1,
                    CreatedByUserId = 3,
                    Status = "verified",
                    CreatedAt = now,
                    UpdatedAt = now
                }
            );

            // =========================
            // REPORTS SEED
            // =========================
            modelBuilder.Entity<Report>().HasData(
                new Report
                {
                    ReportId = 1,
                    UserId = 3,
                    IncidentId = 1,
                    Latitude = 31.9539m,
                    Longitude = 35.2061m,
                    Category = "Accident",
                    Description = "Saw accident",
                    Votes = 5,
                    CreatedAt = now,
                    Status = ReportStatus.Verified
                   
                }
            );

            // =========================
            // VOTES SEED
            // =========================
            modelBuilder.Entity<ReportVote>().HasData(
                new ReportVote
                {
                    Id = 1,
                    ReportId = 1,
                    UserId = 2,
                    IsUpVote = true,
                    VotedAt = now
                }
            );

            // =========================
            // AUDIT LOG SEED
            // =========================
            modelBuilder.Entity<ReportAuditLog>().HasData(
                new ReportAuditLog
                {
                    Id = 1,
                    ReportId = 1,
                    ModeratorId = 2,
                    Action = "Approve",
                    Reason = "Valid",
                    PreviousStatus = "Pending",
                    NewStatus = "Verified",
                    ActionDate = now
                }
            );

            // =========================
            // SUBSCRIPTIONS SEED
            // =========================
            modelBuilder.Entity<Subscription>().HasData(
                new Subscription
                {
                    SubscriptionId = 1,
                    UserId = 3,
                    GeographicArea = "Qalandia",
                    Category = "Accident",
                    CreatedAt = now
                }
            );

            // =========================
            // ROUTES SEED
            // =========================
            modelBuilder.Entity<Route>().HasData(
                new Route
                {
                    RouteId = 1,
                    StartLat = 31.9539m,
                    StartLng = 35.2061m,
                    EndLat = 31.7054m,
                    EndLng = 35.2024m,
                    EstimatedDistance = 25.5m,
                    EstimatedDuration = 0.6m,
                    CreatedAt = now
                }
            );

            // =========================
            // EXTERNAL DATA SEED
            // =========================
            modelBuilder.Entity<ExternalData>().HasData(
                new ExternalData
                {
                    DataId = 1,
                    Source = "WeatherAPI",
                    ExternalKey = "weather_qalandia",
                    JsonData = "{\"temp\":25}",
                    FetchedAt = now
                }
            );

           
            // =========================
            // CHECKPOINT HISTORY SEED
            // =========================
            modelBuilder.Entity<CheckpointStatusHistory>().HasData(
                new CheckpointStatusHistory
                {
                    HistoryId = 1,
                    CheckpointId = 1,
                    Status = "active",
                    ChangedAt = now
                }
            );
        }
    }
}