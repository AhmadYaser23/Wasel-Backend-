namespace ProjectWasel21.Models.ModelsDTO
{
    public class RouteRequestDTO
    {
        public decimal StartLat { get; set; }
        public decimal StartLng { get; set; }
        public decimal EndLat { get; set; }
        public decimal EndLng { get; set; }
        public List<int> AvoidCheckpoints { get; set; } = new List<int>();
        public List<string> AvoidAreas { get; set; } = new List<string>();
    }
}
