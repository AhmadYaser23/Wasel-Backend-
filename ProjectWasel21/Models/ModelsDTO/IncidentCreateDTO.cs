using System.ComponentModel.DataAnnotations;

namespace ProjectWasel21.Models.ModelsDTO
{
    public class IncidentCreateDTO
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^(closure|delay|accident|weather hazard|other)$",
            ErrorMessage = "Type must be: closure, delay, accident, weather hazard, or other.")]
        public string Type { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^(Low|Medium|High|Critical)$",
            ErrorMessage = "Severity must be: Low, Medium, High, or Critical.")]
        public string Severity { get; set; } = string.Empty;

        public int? CheckpointId { get; set; }
    }
}