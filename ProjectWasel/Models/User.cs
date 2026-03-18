using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectWasel.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }  // PK
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public ICollection<Incident> CreatedIncidents { get; set; }
        public ICollection<Incident> VerifiedIncidents { get; set; }
        public ICollection<Report> Reports { get; set; }
        public ICollection<Subscription> Subscriptions { get; set; }
    }
}