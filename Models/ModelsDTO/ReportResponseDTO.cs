using ProjectWasel21.Models.ModelsDTO;

public class ReportResponseDTO
{
    public int Id { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Category { get; set; }
    public string Description { get; set; }

    public string Status { get; set; }
    public int UpVotes { get; set; }

    public UserDTO? User { get; set; }
    public IncidentNestedDTO? Incident { get; set; }
}