using Microsoft.EntityFrameworkCore;
using ProjectWasel21.Models;
using System;
using System.Linq;
using Route = ProjectWasel21.Models.Route;

namespace ProjectWasel21.Data
{
    public class WaselContext : DbContext
    {
        public WaselContext(DbContextOptions<WaselContext> options) : base(options) { }

        // =========================
        // DB SETS
        // =========================
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

        // =========================
        // SEED METHOD (زي المثال الثاني)
        // =========================
        public void Seed()
        {
            var now = DateTime.UtcNow;

            // ================= USERS =================
            if (!Users.Any())
            {
                var admin = new User { Username = "Admin", Email = "admin@wasel.com", PasswordHash = "HASH1", Role = "admin", CreatedAt = now, IsActive = true };
                var mod = new User { Username = "Moderator", Email = "mod@wasel.com", PasswordHash = "HASH2", Role = "moderator", CreatedAt = now, IsActive = true };
                var u1 = new User { Username = "Ahmad", Email = "ahmad@wasel.com", PasswordHash = "HASH3", Role = "citizen", CreatedAt = now, IsActive = true };
                var u2 = new User { Username = "Sara", Email = "sara@wasel.com", PasswordHash = "HASH4", Role = "citizen", CreatedAt = now, IsActive = true };
                var u3 = new User { Username = "Omar", Email = "omar@wasel.com", PasswordHash = "HASH5", Role = "citizen", CreatedAt = now, IsActive = true };

                Users.AddRange(admin, mod, u1, u2, u3);
                SaveChanges();
            }

            var adminUser = Users.First(u => u.Role == "admin");
            var modUser = Users.First(u => u.Role == "moderator");
            var user1 = Users.First(u => u.Email == "ahmad@wasel.com");
            var user2 = Users.First(u => u.Email == "sara@wasel.com");
            var user3 = Users.First(u => u.Email == "omar@wasel.com");

            // ================= CHECKPOINTS =================
            if (!Checkpoints.Any())
            {
                var cp1 = new Checkpoint { Name = "Qalandia", Latitude = 31.9539m, Longitude = 35.2061m, Status = "active", LastUpdated = now };
                var cp2 = new Checkpoint { Name = "Bethlehem", Latitude = 31.7054m, Longitude = 35.2024m, Status = "active", LastUpdated = now };
                var cp3 = new Checkpoint { Name = "Ramallah North", Latitude = 31.9060m, Longitude = 35.2042m, Status = "delayed", LastUpdated = now };

                Checkpoints.AddRange(cp1, cp2, cp3);
                SaveChanges();
            }

            var cpA = Checkpoints.First(c => c.Name == "Qalandia");
            var cpB = Checkpoints.First(c => c.Name == "Bethlehem");
            var cpC = Checkpoints.First(c => c.Name == "Ramallah North");

            // ================= INCIDENTS =================
            if (!Incidents.Any())
            {
                var inc1 = new Incident
                {
                    Title = "Major Accident",
                    Description = "Serious crash on main road",
                    Type = "accident",
                    Severity = "high",
                    CheckpointId = cpA.CheckpointId,
                    CreatedByUserId = adminUser.UserId,
                    Status = "verified",
                    CreatedAt = now,
                    UpdatedAt = now
                };

                var inc2 = new Incident
                {
                    Title = "Road Closure",
                    Description = "Checkpoint temporarily closed",
                    Type = "closure",
                    Severity = "medium",
                    CheckpointId = cpB.CheckpointId,
                    CreatedByUserId = modUser.UserId,
                    Status = "pending",
                    CreatedAt = now,
                    UpdatedAt = now
                };

                Incidents.AddRange(inc1, inc2);
                SaveChanges();
            }

            var incident1 = Incidents.First(i => i.Title == "Major Accident");
            var incident2 = Incidents.First(i => i.Title == "Road Closure");

            // ================= REPORTS =================
            if (!Reports.Any())
            {
                var r1 = new Report
                {
                    UserId = user1.UserId,
                    IncidentId = incident1.IncidentId,
                    Latitude = 31.9539m,
                    Longitude = 35.2061m,
                    Category = "Accident",
                    Description = "I saw a big accident",
                    Status = ReportStatus.Pending,
                    CreatedAt = now
                };

                var r2 = new Report
                {
                    UserId = user2.UserId,
                    IncidentId = incident1.IncidentId,
                    Latitude = 31.9538m,
                    Longitude = 35.2060m,
                    Category = "Traffic",
                    Description = "Heavy traffic due to accident",
                    Status = ReportStatus.Verified,
                    CreatedAt = now
                };

                var r3 = new Report
                {
                    UserId = user3.UserId,
                    IncidentId = incident2.IncidentId,
                    Latitude = 31.7054m,
                    Longitude = 35.2024m,
                    Category = "Closure",
                    Description = "Road completely blocked",
                    Status = ReportStatus.Rejected,
                    CreatedAt = now
                };

                Reports.AddRange(r1, r2, r3);
                SaveChanges();
            }

            var rep1 = Reports.First(r => r.Description.Contains("accident"));
            var rep2 = Reports.First(r => r.Category == "Traffic");
            var rep3 = Reports.First(r => r.Category == "Closure");

            // ================= VOTES =================
            if (!ReportVotes.Any())
            {
                ReportVotes.AddRange(
                    new ReportVote { ReportId = rep1.ReportId, UserId = user2.UserId, IsUpVote = true, VotedAt = now },
                    new ReportVote { ReportId = rep1.ReportId, UserId = user3.UserId, IsUpVote = true, VotedAt = now },

                    new ReportVote { ReportId = rep2.ReportId, UserId = user1.UserId, IsUpVote = false, VotedAt = now },
                    new ReportVote { ReportId = rep2.ReportId, UserId = user3.UserId, IsUpVote = true, VotedAt = now }
                );

                SaveChanges();
            }

            // ================= AUDIT LOGS (MODERATION) =================
            if (!ReportAuditLogs.Any())
            {
                ReportAuditLogs.AddRange(
                    new ReportAuditLog
                    {
                        ReportId = rep1.ReportId,
                        ModeratorId = modUser.UserId,
                        Action = "Review",
                        Reason = "Valid incident",
                        PreviousStatus = "Pending",
                        NewStatus = "Verified",
                        ActionDate = now
                    },
                    new ReportAuditLog
                    {
                        ReportId = rep2.ReportId,
                        ModeratorId = modUser.UserId,
                        Action = "Approve",
                        Reason = "Confirmed by users",
                        PreviousStatus = "Pending",
                        NewStatus = "Verified",
                        ActionDate = now
                    },
                    new ReportAuditLog
                    {
                        ReportId = rep3.ReportId,
                        ModeratorId = modUser.UserId,
                        Action = "Reject",
                        Reason = "Spam or invalid",
                        PreviousStatus = "Pending",
                        NewStatus = "Rejected",
                        ActionDate = now
                    }
                );

                SaveChanges();
            }

            // ================= SUBSCRIPTIONS =================
            if (!Subscriptions.Any())
            {
                Subscriptions.AddRange(
                    new Subscription { UserId = user1.UserId, GeographicArea = "Qalandia", Category = "Accident", CreatedAt = now },
                    new Subscription { UserId = user2.UserId, GeographicArea = "Bethlehem", Category = "Traffic", CreatedAt = now },
                    new Subscription { UserId = user3.UserId, GeographicArea = "Ramallah", Category = "Closure", CreatedAt = now }
                );

                SaveChanges();
            }

            // ================= ROUTES =================
            if (!Routes.Any())
            {
                Routes.AddRange(
                    new Route
                    {
                        StartLat = 31.9539m,
                        StartLng = 35.2061m,
                        EndLat = 31.7054m,
                        EndLng = 35.2024m,
                        EstimatedDistance = 25.5m,
                        EstimatedDuration = 0.6m,
                        CreatedAt = now
                    },
                    new Route
                    {
                        StartLat = 31.9060m,
                        StartLng = 35.2042m,
                        EndLat = 31.9539m,
                        EndLng = 35.2061m,
                        EstimatedDistance = 18.2m,
                        EstimatedDuration = 0.4m,
                        CreatedAt = now
                    }
                );

                SaveChanges();
            }

            // ================= EXTERNAL DATA =================
            if (!ExternalData.Any())
            {
                ExternalData.AddRange(
                    new ExternalData { Source = "WeatherAPI", ExternalKey = "qalandia_weather", JsonData = "{\"temp\":25}", FetchedAt = now },
                    new ExternalData { Source = "WeatherAPI", ExternalKey = "bethlehem_weather", JsonData = "{\"temp\":18}", FetchedAt = now }
                );

                SaveChanges();
            }

            // ================= CHECKPOINT HISTORY =================
            if (!CheckpointStatusHistories.Any())
            {
                CheckpointStatusHistories.AddRange(
                    new CheckpointStatusHistory { CheckpointId = cpA.CheckpointId, Status = "active", ChangedAt = now },
                    new CheckpointStatusHistory { CheckpointId = cpB.CheckpointId, Status = "active", ChangedAt = now },
                    new CheckpointStatusHistory { CheckpointId = cpC.CheckpointId, Status = "delayed", ChangedAt = now }
                );

                SaveChanges();
            }
        }

        // =========================
        // RELATIONS + CONFIG
        // =========================
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================
            // PRECISION
            // =========================
            modelBuilder.Entity<Report>()
                .Property(x => x.Latitude)
                .HasPrecision(9, 6)
                .IsRequired();

            modelBuilder.Entity<Report>()
                .Property(x => x.Longitude)
                .HasPrecision(9, 6)
                .IsRequired();

            modelBuilder.Entity<Route>()
                .Property(x => x.StartLat).HasPrecision(9, 6).IsRequired();

            modelBuilder.Entity<Route>()
                .Property(x => x.StartLng).HasPrecision(9, 6).IsRequired();

            modelBuilder.Entity<Route>()
                .Property(x => x.EndLat).HasPrecision(9, 6).IsRequired();

            modelBuilder.Entity<Route>()
                .Property(x => x.EndLng).HasPrecision(9, 6).IsRequired();

            // =========================
            // USER
            // =========================
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .IsRequired();

            // =========================
            // INCIDENT
            // =========================
            modelBuilder.Entity<Incident>()
                .HasOne(i => i.CreatedByUser)
                .WithMany(u => u.CreatedIncidents)
                .HasForeignKey(i => i.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<Incident>()
                .HasOne(i => i.Checkpoint)
                .WithMany(c => c.Incidents)
                .HasForeignKey(i => i.CheckpointId)
                .OnDelete(DeleteBehavior.Restrict) // ❗ بدل SetNull
                .IsRequired();

            // =========================
            // REPORT
            // =========================
            modelBuilder.Entity<Report>()
     .HasOne(r => r.User)
     .WithMany(u => u.Reports)
     .HasForeignKey(r => r.UserId)
     .OnDelete(DeleteBehavior.Restrict)
     .IsRequired();

            modelBuilder.Entity<Report>()
                .HasOne(r => r.Incident)
                .WithMany(i => i.Reports)
                .HasForeignKey(r => r.IncidentId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            // Votes
            modelBuilder.Entity<ReportVote>()
                .HasOne(v => v.Report)
                .WithMany(r => r.Votes)
                .HasForeignKey(v => v.ReportId)
                .OnDelete(DeleteBehavior.Cascade);

            // Audit Logs
            modelBuilder.Entity<ReportAuditLog>()
                .HasOne(a => a.Report)
                .WithMany(r => r.AuditLogs)
                .HasForeignKey(a => a.ReportId)
                .OnDelete(DeleteBehavior.Cascade);

            // Moderator
            modelBuilder.Entity<ReportAuditLog>()
                .HasOne(a => a.Moderator)
                .WithMany()
                .HasForeignKey(a => a.ModeratorId)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // SUBSCRIPTION
            // =========================
            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.User)
                .WithMany(u => u.Subscriptions)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<Subscription>()
                .Property(s => s.GeographicArea)
                .IsRequired();

            modelBuilder.Entity<Subscription>()
                .Property(s => s.Category)
                .IsRequired();

            // =========================
            // REFRESH TOKEN
            // =========================
            modelBuilder.Entity<RefreshToken>()
                .HasOne(r => r.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            // =========================
            // CHECKPOINT HISTORY
            // =========================
            modelBuilder.Entity<CheckpointStatusHistory>()
                .HasOne(h => h.Checkpoint)
                .WithMany(c => c.StatusHistory)
                .HasForeignKey(h => h.CheckpointId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            // =========================
            // REPORT AUDIT LOG
            // =========================
            modelBuilder.Entity<ReportAuditLog>()
                .HasOne(a => a.Report)
                .WithMany(r => r.AuditLogs)
                .HasForeignKey(a => a.ReportId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<ReportAuditLog>()
                .HasOne(a => a.Moderator)
                .WithMany()
                .HasForeignKey(a => a.ModeratorId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
    }
}