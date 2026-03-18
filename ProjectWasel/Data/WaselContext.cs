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
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<ExternalData> ExternalData { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Route> Routes { get; set; } // جدول المسارات الجديد

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var now = DateTime.UtcNow;

            // ======= Decimal Precision =======
            modelBuilder.Entity<Checkpoint>().Property(c => c.Latitude).HasPrecision(9, 6);
            modelBuilder.Entity<Checkpoint>().Property(c => c.Longitude).HasPrecision(9, 6);
            modelBuilder.Entity<Report>().Property(r => r.Latitude).HasPrecision(9, 6);
            modelBuilder.Entity<Report>().Property(r => r.Longitude).HasPrecision(9, 6);
            modelBuilder.Entity<Route>().Property(r => r.StartLat).HasPrecision(9, 6);
            modelBuilder.Entity<Route>().Property(r => r.StartLng).HasPrecision(9, 6);
            modelBuilder.Entity<Route>().Property(r => r.EndLat).HasPrecision(9, 6);
            modelBuilder.Entity<Route>().Property(r => r.EndLng).HasPrecision(9, 6);
            modelBuilder.Entity<Route>().Property(r => r.EstimatedDistance).HasPrecision(10, 2);
            modelBuilder.Entity<Route>().Property(r => r.EstimatedDuration).HasPrecision(10, 2);

            // ======= Relationships =======
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

            modelBuilder.Entity<CheckpointStatusHistory>()
                .HasOne(h => h.Checkpoint)
                .WithMany(c => c.StatusHistory)
                .HasForeignKey(h => h.CheckpointId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.Incident)
                .WithMany(i => i.Reports)
                .HasForeignKey(r => r.IncidentId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reports)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.User)
                .WithMany(u => u.Subscriptions)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ======= Seed Data =======
            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, Username = "Ahmed_Admin", Email = "ahmed@wasel.ps", PasswordHash = "SECRET_HASH_ABC", Role = "admin", CreatedAt = now },
                new User { UserId = 2, Username = "Noor_Mod", Email = "noor@wasel.ps", PasswordHash = "SECRET_HASH_XYZ", Role = "moderator", CreatedAt = now },
                new User { UserId = 3, Username = "Mohammad_User", Email = "mohammad@wasel.ps", PasswordHash = "SECRET_HASH_123", Role = "citizen", CreatedAt = now }
            );

            modelBuilder.Entity<Checkpoint>().HasData(
                new Checkpoint { CheckpointId = 1, Name = "Qalandia", Latitude = 31.9539m, Longitude = 35.2061m, Status = "active", LastUpdated = now },
                new Checkpoint { CheckpointId = 2, Name = "Bethlehem 300", Latitude = 31.7054m, Longitude = 35.2024m, Status = "delayed", LastUpdated = now }
            );

            modelBuilder.Entity<CheckpointStatusHistory>().HasData(
                new CheckpointStatusHistory { HistoryId = 1, CheckpointId = 1, Status = "active", ChangedAt = now },
                new CheckpointStatusHistory { HistoryId = 2, CheckpointId = 2, Status = "delayed", ChangedAt = now }
            );

            modelBuilder.Entity<Incident>().HasData(
                new Incident { IncidentId = 1, Title = "Accident at Qalandia", Description = "Minor accident", Type = "accident", Severity = "low", CheckpointId = 1, CreatedByUserId = 3, Status = "verified", CreatedAt = now, UpdatedAt = now },
                new Incident { IncidentId = 2, Title = "Closure at Bethlehem 300", Description = "Temporary closure", Type = "closure", Severity = "medium", CheckpointId = 2, CreatedByUserId = 3, Status = "verified", CreatedAt = now, UpdatedAt = now }
            );

            modelBuilder.Entity<Report>().HasData(
                new Report { ReportId = 1, UserId = 3, IncidentId = 1, Latitude = 31.9539m, Longitude = 35.2061m, Category = "Accident", Description = "Saw minor accident", Votes = 5, CreatedAt = now },
                new Report { ReportId = 2, UserId = 2, IncidentId = 2, Latitude = 31.7054m, Longitude = 35.2024m, Category = "Closure", Description = "Road closed temporarily", Votes = 3, CreatedAt = now }
            );

            modelBuilder.Entity<Subscription>().HasData(
                new Subscription { SubscriptionId = 1, UserId = 3, GeographicArea = "Qalandia", Category = "Accident", CreatedAt = now },
                new Subscription { SubscriptionId = 2, UserId = 2, GeographicArea = "Bethlehem", Category = "Closure", CreatedAt = now }
            );

            modelBuilder.Entity<ExternalData>().HasData(
                new ExternalData { DataId = 1, Source = "WeatherAPI", ExternalKey = "weather_1", JsonData = "{\"temp\":25,\"status\":\"sunny\"}", FetchedAt = now },
                new ExternalData { DataId = 2, Source = "OpenStreetMap", ExternalKey = "map_1", JsonData = "{\"road\":\"open\"}", FetchedAt = now }
            );

            // ======= Seed Data for Routes =======
            modelBuilder.Entity<Route>().HasData(
                new Route
                {
                    RouteId = 1,
                    StartLat = 31.9539m,
                    StartLng = 35.2061m,
                    EndLat = 31.7054m,
                    EndLng = 35.2024m,
                    EstimatedDistance = 30.5m,
                    EstimatedDuration = 0.76m,
                    CreatedAt = now
                }
            );
        }
    }
}