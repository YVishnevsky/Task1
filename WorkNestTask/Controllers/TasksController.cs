using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using WorkNestTask.Filters;
using WorkNestTask.Models;
using WorkNestTask.Requests;
using System.Security.Claims;

namespace WorkNestTask.Controllers
{
    [RoutePrefix("api/tasks")]
    [JwtAuthentication]
    public class TasksController : ApiController
    {
        readonly TaskDbContext _dbContext;

        public TasksController(TaskDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> AddNew(NewTask taskRequest)
        {
            string userId = ((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var task = new Models.Task
            {
                AssigneeId = taskRequest.AssigneeId,
                Created = DateTime.Now,
                Name = taskRequest.Name,
                ReporterId = userId
            };

            _dbContext.Tasks.Add(task);

            await _dbContext.SaveChangesAsync();

            return Created($"tasks/{task.Id}", new { id = task.Id });
        }

        [Route("{id}")]
        public async Task<IHttpActionResult> GetTask(int id)
        {
            var viewmodel = await _dbContext.Tasks
                .Include(t => t.Reporter)
                .Include(t => t.Assignee)
                .Select(t => new
                {
                    t.Id,
                    t.Name,
                    Reporter = new { t.ReporterId, t.Reporter.UserName },
                    t.Created,
                    Assign = new { Id = t.AssigneeId, t.Assignee.UserName },
                    t.Completed
                }).FirstAsync(t => t.Id == id);

            return Ok(viewmodel);
        }

        [Route("")]
        public async Task<IHttpActionResult> GetTasks()
        {
            var viewmodel = await _dbContext.Tasks
                .Include(t => t.Reporter)
                .Include(t => t.Assignee)
                .Select(t => new
                {
                    t.Id,
                    t.Name,
                    Reporter = new { t.ReporterId, t.Reporter.UserName },
                    t.Created,
                    Assign = new { Id = t.AssigneeId, t.Assignee.UserName },
                    t.Completed
                })
                .OrderBy(t => t.Completed).ThenByDescending(t => t.Created)
                .ToListAsync();

            return Ok(viewmodel);
        }

        [Route("{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateTasks(int id, [FromBody]UpdateTask updateTask)
        {
            var task = await _dbContext.Tasks.FindAsync(id);
            _dbContext.Entry(task).CurrentValues.SetValues(updateTask);

            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [Route("{id}")]
        public async System.Threading.Tasks.Task Delete(int id)
        {
            var task = new Models.Task { Id = id };
            _dbContext.Tasks.Attach(task);
            _dbContext.Tasks.Remove(task);

            await _dbContext.SaveChangesAsync();
        }
    }
}
