// Models/ModelsDTO/ReportDTO.cs
using System.ComponentModel.DataAnnotations;

namespace ProjectWasel21.Models.ModelsDTO
{

    public class CreateReportDTO
    {
        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        [MinLength(10)]
        public string Description { get; set; } = string.Empty;

        // 🔴 مهم جداً لحل مشكلتك
        [Required]
        public int IncidentId { get; set; }
    }



    public class ReportResponseDTO
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string Status { get; set; } = string.Empty;
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
        public double ConfidenceScore { get; set; }
        public string SubmittedBy { get; set; } = string.Empty;
    }

    public class ModerationDTO
    {
        [Required]
        public string Action { get; set; } = string.Empty; // Approve, Reject, MarkDuplicate

        [MaxLength(500)]
        public string? Reason { get; set; }
    }

    public class VoteDTO
    {
        [Required]
        public bool IsUpVote { get; set; }
    }

    // Models/ModelsDTO/ReportDTO.cs
    // أضف هذا الـ class في نهاية الملف

    public class AuditLogResponseDTO
    {
        public int Id { get; set; }
        public string Action { get; set; } = string.Empty;
        public string? Reason { get; set; }
        public string PreviousStatus { get; set; } = string.Empty;
        public string NewStatus { get; set; } = string.Empty;
        public DateTime ActionDate { get; set; }
        public string ModeratorName { get; set; } = string.Empty;


        // وفي ReportResponseDTO الموجودة أضف هذا الحقل
        public string ConfidenceLabel { get; set; } = string.Empty;
    }
}