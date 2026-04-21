using System.ComponentModel.DataAnnotations;

namespace ProjectWasel21.Models.ModelsDTO
{
    public class CheckpointDTO
    {
        [Required(ErrorMessage = "Checkpoint name is required.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 200 characters.")]
        public string Name { get; set; }

        [Required]
        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90.")]
        public decimal Latitude { get; set; }

        [Required]
        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180.")]
        public decimal Longitude { get; set; }

        [RegularExpression("^(active|delayed|closed|restricted)$",
            ErrorMessage = "Status must be: active, delayed, closed, or restricted.")]
        public string Status { get; set; } = "active";
    }
}
