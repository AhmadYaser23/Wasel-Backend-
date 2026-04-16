// Models/ModelsDTO/ReportDTO.cs
using System.ComponentModel.DataAnnotations;

namespace ProjectWasel21.Models.ModelsDTO
{
    public class CreateReportDTO
    {
        [Required(ErrorMessage = "Location required")]
        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90")]
        public double Latitude { get; set; }

        [Required]
        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180")]
        public double Longitude { get; set; }

        [Required(ErrorMessage = "Category required")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description required")]
        [MinLength(10, ErrorMessage = "Description must be at least 10 characters")]
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;
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