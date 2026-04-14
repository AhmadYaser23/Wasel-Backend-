namespace ProjectWasel.Models.ModelsDTO
{
    public class IncidentDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Severity { get; set; }

        public int? CheckpointId { get; set; }
        public int? CreatedByUserId { get; set; }

        public List<int> ReportIds { get; set; } = new();
    }
}