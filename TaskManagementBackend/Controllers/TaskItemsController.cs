using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using TaskManagementBackend.Context;
using TaskManagementBackend.DTOs;
using TaskManagementBackend.Models;

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

        // GET: api/TaskItems
        [HttpGet]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<IEnumerable<TaskItemDTO>>> GetTaskItems()
        {
            var taskItems = await _context.TaskItems.ToListAsync();
            var taskItemsDto = mapper.Map<List<TaskItemDTO>>(taskItems);
            return taskItemsDto;
        }

        // GET: api/TaskItems/5
        [HttpGet("{id}")]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<TaskItemDTO>> GetTaskItem(long id)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);

            if (taskItem == null)
            {
                return NotFound();
            }

            var taskItemDto = mapper.Map<TaskItemDTO>(taskItem);

            return taskItemDto;
        }

        // PUT: api/TaskItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskItem(long id, TaskItem taskItem)
        {
            if (id != taskItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(taskItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TaskItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostTaskItem([FromBody] CreateTaskItemDto createTaskDto)
        {
            var taskItem = mapper.Map<TaskItem>(createTaskDto);
            _context.TaskItems.Add(taskItem);
            await _context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default); 

            //return CreatedAtAction("GetTaskItem", new { id = taskItem.Id }, taskItem);
            return CreatedAtAction(nameof(GetTaskItem), new { id = taskItem.Id }, taskItem);
        }

        // DELETE: api/TaskItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskItem(long id)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem == null)
            {
                return NotFound();
            }

            _context.TaskItems.Remove(taskItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskItemExists(long id)
        {
            return _context.TaskItems.Any(e => e.Id == id);
        }
    }
}
