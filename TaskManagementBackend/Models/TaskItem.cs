namespace TaskManagementBackend.Models
{
    public class TaskItem
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool? IsComplete { get; set; }

    }
}
