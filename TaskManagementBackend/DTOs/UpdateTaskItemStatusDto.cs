using System.ComponentModel.DataAnnotations;

namespace TaskManagementBackend.DTOs
{
    public class UpdateTaskItemStatusDto
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public required bool IsComplete { get; set; }
    }
}
