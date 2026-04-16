using ProjectWasel21.Models;

public class Report
{
    public int ReportId { get; set; }

    public int UserId { get; set; }
    public int IncidentId { get; set; }

    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }

    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public ReportStatus Status { get; set; } = ReportStatus.Pending;

    // =========================
    // RELATIONS
    // =========================
    public User User { get; set; } = null!;
    public Incident Incident { get; set; } = null!;

    public ICollection<ReportVote> Votes { get; set; } = new List<ReportVote>();

    public ICollection<ReportAuditLog> AuditLogs { get; set; } = new List<ReportAuditLog>();
}

public enum ReportStatus
{
    Pending,
    Verified,
    Rejected,
    Duplicate
}