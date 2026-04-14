using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectWasel.Models
{
    public class Alert
    {
        [Key]
        public int AlertId { get; set; }

        public int IncidentId { get; set; }

        public string Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}



