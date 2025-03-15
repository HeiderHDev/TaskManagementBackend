using System.ComponentModel.DataAnnotations;

namespace TaskManagementBackend.DTOs
{
    public class CreateTaskItemDto
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(500, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        public required string Description { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public required bool IsComplete { get; set; }
    }
}
