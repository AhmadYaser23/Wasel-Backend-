namespace ProjectWasel21.Models.ModelsDTO
{
    public class AlertDTO
    {
        public int AlertId { get; set; }

        public int IncidentId { get; set; }

        public string Message { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}