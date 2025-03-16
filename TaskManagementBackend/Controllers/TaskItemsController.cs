using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using TaskManagementBackend.Context;
using TaskManagementBackend.DTOs;
using TaskManagementBackend.Models;
using TaskManagementBackend.Utils;

namespace TaskManagementBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskItemsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IOutputCacheStore outputCacheStore;
        private readonly IMapper mapper;
        private const string cacheTag = "TaskItems";

        public TaskItemsController(AppDbContext context, IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            _context = context;
            this.outputCacheStore = outputCacheStore;
            this.mapper = mapper;
        }

        [HttpGet]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<IEnumerable<TaskItemDTO>>> GetTaskItems([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.TaskItems;
            await HttpContext.InsertPaginationParametersInHeader(queryable);
            return await queryable
                .OrderByDescending(task => task.Id)
                .Paginate(pagination)
                .ProjectTo<TaskItemDTO>(mapper.ConfigurationProvider).ToListAsync();
        }

        [HttpGet("{id}")]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<TaskItemDTO>> GetTaskItem(long id)
        {
            var taskItem = await _context.TaskItems
                .ProjectTo<TaskItemDTO>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(task => task.Id == id);

            if (taskItem == null)
            {
                return NotFound();
            }

            return taskItem;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskItem(long id, [FromBody] CreateTaskItemDto createTaskItemDto)
        {
            var taskItemExist = await _context.TaskItems.AnyAsync(task => task.Id == id);
            if (!taskItemExist) 
            {
                return NotFound();
            }

            var taskItem = mapper.Map<TaskItem>(createTaskItemDto);
            taskItem.Id = id;
            _context.Update(taskItem);
            await _context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> PostTaskItem([FromBody] CreateTaskItemDto createTaskDto)
        {
            var taskItem = mapper.Map<TaskItem>(createTaskDto);
            _context.TaskItems.Add(taskItem);
            await _context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default); 
            return CreatedAtAction(nameof(GetTaskItem), new { id = taskItem.Id }, taskItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskItem(long id)
        {
            var recordsDeleted = await _context.TaskItems.Where(task => task.Id == id).ExecuteDeleteAsync();
            if (recordsDeleted == 0) {  return NotFound(); }
            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            return NoContent();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> PatchTaskItemStatus(long id, [FromBody] UpdateTaskItemStatusDto updateTaskItemStatusDto)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem == null)
            {
                return NotFound();
            }

            taskItem.IsComplete = updateTaskItemStatusDto.IsComplete;
            _context.Entry(taskItem).Property(t => t.IsComplete).IsModified = true;
            await _context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            return NoContent();
        }

        private bool TaskItemExists(long id)
        {
            return _context.TaskItems.Any(e => e.Id == id);
        }
    }
}
