using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectWasel.Models
{
    public class Route
    {
        [Key]
        public int RouteId { get; set; }

        public decimal StartLat { get; set; }
        public decimal StartLng { get; set; }

        public decimal EndLat { get; set; }
        public decimal EndLng { get; set; }

        public decimal EstimatedDistance { get; set; }

        public decimal EstimatedDuration { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}