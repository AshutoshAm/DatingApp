using DatingApp.Data;
using DatingApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext context;
        public UsersController(DataContext Context)
        {
            context = Context;
        }

        [HttpGet]
        [Route("api/test")]
        public ActionResult Test()
        {
            context.Add(new AppUser { Id = 1, UserName = "Ashutosh" });
            context.Add(new AppUser { Id = 2, UserName = "Deepak" });
            context.SaveChanges();
            return Ok();
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            return Ok(context.Users.ToList());
        }

    }
}
