using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectWasel.Models
{
    public class Report
    {
        [Key]
        public int ReportId { get; set; } // PK
        public int? UserId { get; set; }
        public int? IncidentId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public int Votes { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("IncidentId")]
        public Incident Incident { get; set; }
    }
}