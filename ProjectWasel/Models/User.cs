using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectWasel.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }  // PK

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        // Role: admin / moderator / citizen
        [Required]
        public string Role { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        public int FailedLoginAttempts { get; set; } = 0;

        public DateTime? LockoutUntil { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LastLoginAt { get; set; }

        // ===== Refresh Tokens (JWT) =====
        public ICollection<RefreshToken> RefreshTokens { get; set; }

        // ===== Relations =====
        public ICollection<Incident> CreatedIncidents { get; set; }
        public ICollection<Incident> VerifiedIncidents { get; set; }
        public ICollection<Report> Reports { get; set; }
        public ICollection<ReportVote> Votes { get; set; }
        public ICollection<Subscription> Subscriptions { get; set; }
    }
}