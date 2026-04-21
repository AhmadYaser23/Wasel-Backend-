namespace ProjectWasel21.Models.ModelsDTO
{
    public class SubscriptionDTO
    {
        public int SubscriptionId { get; set; }
        public int UserId { get; set; }
        public string GeographicArea { get; set; }
        public string Category { get; set; }
        public DateTime CreatedAt { get; set; }

        public UserDTO User { get; set; }
    }
}