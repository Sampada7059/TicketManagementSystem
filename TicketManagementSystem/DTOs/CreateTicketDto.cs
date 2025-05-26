using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.DTOs
{
    public class CreateTicketDto
    {
        [Required]
        [StringLength(100)]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [RegularExpression("Low|Medium|High")]
        public string Priority { get; set; } = "Low";
    }

}
