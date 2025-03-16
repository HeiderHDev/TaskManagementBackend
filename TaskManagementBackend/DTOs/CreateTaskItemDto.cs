using System.ComponentModel.DataAnnotations;

namespace TaskManagementBackend.DTOs
{
    public class CreateTaskItemDto
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(300, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        [MinLength(10, ErrorMessage = "El campo {0} debe tener al menos {1} caracteres")]
        public required string Description { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public required bool IsComplete { get; set; }
    }
}
