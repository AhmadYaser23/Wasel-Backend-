using ProjectWasel21.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectWasel21.Models
{
    public class Alert
    {
        [Key]
        public int AlertId { get; set; }

        public int IncidentId { get; set; }

        public string Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Incident Incident { get; set; }
    }
}