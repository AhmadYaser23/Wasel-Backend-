using ProjectWasel.Models.ModelsDTO;

public class IncidentNestedDTO
{
    public int IncidentId { get; set; }
    public string Title { get; set; }
    public string Type { get; set; }

    public CheckpointDTO? Checkpoint { get; set; }
}