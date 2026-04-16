using ProjectWasel21.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectWasel21.Models
{
    public class Incident
    {
        [Key]
        public int IncidentId { get; set; }      // PK
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Severity { get; set; }

        public int? CheckpointId { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? VerifiedByUserId { get; set; }

        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation
        public Checkpoint Checkpoint { get; set; }

        [ForeignKey("CreatedByUserId")]
        public User CreatedByUser { get; set; }

        [ForeignKey("VerifiedByUserId")]
        public User? VerifiedByUser { get; set; }

        public ICollection<Report> Reports { get; set; }
        public ICollection<Alert> Alerts { get; set; }
    }
}