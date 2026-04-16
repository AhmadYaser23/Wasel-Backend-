// Models/ReportAuditLog.cs

using ProjectWasel21.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectWasel21.Models
{
    public class ReportAuditLog
    {
        [Key]
        public int Id { get; set; }

        public int ReportId { get; set; }

        [ForeignKey("ReportId")]
        public Report? Report { get; set; }

        public int ModeratorId { get; set; }

        [ForeignKey("ModeratorId")]
        public User? Moderator { get; set; }

        [Required]
        public string Action { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Reason { get; set; }

        public string PreviousStatus { get; set; } = string.Empty;
        public string NewStatus { get; set; } = string.Empty;

        public DateTime ActionDate { get; set; } = DateTime.UtcNow;
    }
}