using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WorkNestTask.Filters;
using WorkNestTask.Models;

namespace WorkNestTask.Controllers
{
    [RoutePrefix("api/users")]
    [JwtAuthentication]
    public class UsersController : ApiController
    {
        readonly TaskDbContext _dbContext;

        public UsersController(TaskDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [Route("")]
        public async Task<IHttpActionResult> GetList() => Ok(await _dbContext.Users.Select(u => new { u.Id, u.UserName }).ToListAsync());
    }
}
