namespace ProjectWasel.Models.ModelsDTO
{
    public class IncidentResponseDTO
    {
        public int IncidentId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        public int? CheckpointId { get; set; }
        public string? CheckpointName { get; set; }

        public int? CreatedByUserId { get; set; }
        public string? CreatedByUsername { get; set; }

        public int? VerifiedByUserId { get; set; }
        public string? VerifiedByUsername { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
