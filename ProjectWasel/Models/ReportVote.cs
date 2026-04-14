// Models/ReportVote.cs

using ProjectWasel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectWasel.Models
{
    public class ReportVote
    {
        [Key]
        public int Id { get; set; }

        public int ReportId { get; set; }

        [ForeignKey("ReportId")]
        public Report? Report { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        public bool IsUpVote { get; set; }

        public DateTime VotedAt { get; set; } = DateTime.UtcNow;
    }
}