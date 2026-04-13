using System.ComponentModel.DataAnnotations;

namespace ProjectWasel.Models.ModelsDTO
{
    public class CheckpointUpdateDTO
    {
        [Required(ErrorMessage = "Status is required.")]
        [RegularExpression("^(active|delayed|closed|restricted)$",
            ErrorMessage = "Status must be: active, delayed, closed, or restricted.")]
        public string Status { get; set; } = string.Empty;
    }
}
