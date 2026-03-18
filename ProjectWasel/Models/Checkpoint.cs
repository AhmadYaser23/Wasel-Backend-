using ProjectWasel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectWasel.Models
{
    public class Checkpoint
    {
        [Key]
        public int CheckpointId { get; set; } // PK
        public string Name { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Status { get; set; }
        public DateTime LastUpdated { get; set; }

        // Navigation
        public ICollection<Incident> Incidents { get; set; }
        public ICollection<CheckpointStatusHistory> StatusHistory { get; set; }
    }
}