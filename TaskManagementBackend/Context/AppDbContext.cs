using Microsoft.EntityFrameworkCore;
using TaskManagementBackend.Models;

namespace TaskManagementBackend.Context
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<TaskItem> TaskItems { get; set; }


    }
}
