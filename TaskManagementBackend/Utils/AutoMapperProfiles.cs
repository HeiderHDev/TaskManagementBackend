using AutoMapper;
using TaskManagementBackend.DTOs;
using TaskManagementBackend.Models;

namespace TaskManagementBackend.Utils
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            ConfigurarMapeoTareas();
        }

        private void ConfigurarMapeoTareas()
        {
            CreateMap<CreateTaskItemDto, TaskItem>();
            CreateMap<TaskItem, TaskItemDTO>();
        }
    }
}
