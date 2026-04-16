using ProjectWasel21.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectWasel21.Models
{
    public class Checkpoint
    {
        [Key]
        public int CheckpointId { get; set; } // PK
        public string Name { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Status { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        // Navigation
        public ICollection<Incident> Incidents { get; set; }

        public List<CheckpointStatusHistory> StatusHistory { get; set; } = new List<CheckpointStatusHistory>();
    }
}