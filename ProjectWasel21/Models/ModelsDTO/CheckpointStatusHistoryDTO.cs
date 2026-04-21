namespace ProjectWasel21.Models.ModelsDTO
{
    public class CheckpointStatusHistoryDTO
    {
        public int HistoryId { get; set; }
        public string Status { get; set; }
        public DateTime ChangedAt { get; set; }
        public DateTime LastUpdated { get; set; }
        public string CheckpointName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public List<IncidentSummaryDTO> Incidents { get; set; } = new();
    }

    public class IncidentSummaryDTO
    {
        public int IncidentId { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Severity { get; set; }
        public string Status { get; set; }
    }
}