using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectWasel.Models
{
    public class CheckpointStatusHistory
    {
        [Key]

        public int HistoryId { get; set; } // PK
        public int CheckpointId { get; set; }
        public string Status { get; set; }
        public DateTime ChangedAt { get; set; }

        [ForeignKey("CheckpointId")]
        public Checkpoint? Checkpoint { get; set; }
        
    }
}