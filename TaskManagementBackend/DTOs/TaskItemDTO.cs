using System.ComponentModel.DataAnnotations;

namespace TaskManagementBackend.DTOs
{
    public class TaskItemDTO
    {
        public long Id { get; set; }

        public required string Title { get; set; }

        public required string Description { get; set; }

        public required bool IsComplete { get; set; }

        // Campos de auditoría
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; } = null;

        // Campo para soft delete
        public bool IsDeleted { get; set; } = false;
    }
}
