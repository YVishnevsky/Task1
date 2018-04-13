using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WorkNestTask.Jwt;
using WorkNestTask.Models;
using WorkNestTask.Requests;

namespace WorkNestTask.Controllers
{
    [RoutePrefix("api/signin")]
    public class SignInController : ApiController
    {
        UserManager<ApplicationUser> _userManager;

        public SignInController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> SignIn(SignIn signinRequest)
        {
            var user = await _userManager.FindAsync(signinRequest.UserName, signinRequest.Password);
            if (user != null)
            {
                string token = JwtManager.GenerateToken(user);

                return Ok(new { token });
            }
            else
                return BadRequest();
        }
    }
}
