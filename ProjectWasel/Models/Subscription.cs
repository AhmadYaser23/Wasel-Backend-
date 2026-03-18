using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectWasel.Models
{
    public class Subscription
    {
        [Key]
        public int SubscriptionId { get; set; } // PK
        public int? UserId { get; set; }
        public string GeographicArea { get; set; }
        public string Category { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}