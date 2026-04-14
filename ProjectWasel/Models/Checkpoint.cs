using ProjectWasel.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Checkpoint
{
    [Key]
    public int CheckpointId { get; set; }

    public string Name { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string Status { get; set; }
    public DateTime LastUpdated { get; set; }

    // 🔥 أهم تعديل: تهيئة القوائم (بدون حذف JsonIgnore)
  //  [JsonIgnore]
    public ICollection<Incident> Incidents { get; set; } = new List<Incident>();

   // [JsonIgnore]
    public ICollection<CheckpointStatusHistory> StatusHistory { get; set; } = new List<CheckpointStatusHistory>();
}