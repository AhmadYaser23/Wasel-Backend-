namespace ProjectWasel.Models.ModelsDTO
{
    public class ReportDTO
    {
        public int? UserId { get; set; }
        public int? IncidentId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
    }
}
