namespace ProjectWasel.Models.ModelsDTO
{
    public class AlertSubscribersDTO
    {
        public int IncidentId { get; set; }

        public List<int> SubscriberUserIds { get; set; } = new List<int>();
    }
}