using ProjectWasel.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Incident
{
    [Key]
    public int IncidentId { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public string Severity { get; set; }

    public int? CheckpointId { get; set; }
    public int? CreatedByUserId { get; set; }

    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // 👇 العلاقات (حل اللوب + تهيئة مثل Checkpoint)

    //[JsonIgnore]
    public Checkpoint? Checkpoint { get; set; }

   // [JsonIgnore]
    public User? CreatedByUser { get; set; }

   // [JsonIgnore]
    public ICollection<Report>? Reports { get; set; } = new List<Report>();

}