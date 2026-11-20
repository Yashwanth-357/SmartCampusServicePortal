using System.ComponentModel.DataAnnotations;

namespace SmartCampusServicePortal.Models
{
    // DTO for updating the status and optional remarks of a service request
    public class StatusUpdateDto
    {
        [Required(ErrorMessage = "Status is required")]
        [StringLength(30)]
        public string Status { get; set; } = string.Empty;

        [StringLength(250)]
        public string? AdminRemarks { get; set; }
    }
}
