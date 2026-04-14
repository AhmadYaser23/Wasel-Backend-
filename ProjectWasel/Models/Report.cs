using ProjectWasel.Models;

public class Report
{
    public int ReportId { get; set; }

    public int? UserId { get; set; }
    public int? IncidentId { get; set; }

    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }

    public string Category { get; set; }
    public string Description { get; set; }

    public int Votes { get; set; }
    public DateTime CreatedAt { get; set; }

    // 🔥 خليه بدون JsonIgnore
    public User? User { get; set; }

    public Incident? Incident { get; set; }

    public ReportStatus Status { get; set; } = ReportStatus.Pending;
}

public enum ReportStatus
{
    Pending,
    Verified,
    Rejected,
    Duplicate
}