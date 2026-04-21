using System.ComponentModel.DataAnnotations;

namespace ProjectWasel21.Models.ModelsDTO
{
    public class IncidentUpdateDTO
    {
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 200 characters.")]
        public string? Title { get; set; }

        public string? Description { get; set; }

        [RegularExpression("^(closure|delay|accident|weather hazard|other)$",
            ErrorMessage = "Type must be: closure, delay, accident, weather hazard, or other.")]
        public string? Type { get; set; }

        [RegularExpression("^(Low|Medium|High|Critical)$",
            ErrorMessage = "Severity must be: Low, Medium, High, or Critical.")]
        public string? Severity { get; set; }

        [RegularExpression("^(active|verified|closed)$",
            ErrorMessage = "Status must be: active, verified, or closed.")]
        public string? Status { get; set; }

        public int? CheckpointId { get; set; }
    }
}