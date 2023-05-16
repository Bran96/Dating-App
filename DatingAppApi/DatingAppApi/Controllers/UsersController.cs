using DatingAppApi.Data;
using DatingAppApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingAppApi.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        // Its better to use underscore '_' for private variables
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        // We want to get a list of users thats why we use "IEnumerable"
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<AppUser>> GetUser([FromRoute] int id)
        {
            // We can use Find -> It must be a PK then, or we can use FirstOrDefault -> that doesnt need to be a PK
            var user = await _context.Users.FindAsync(id);

            if(user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

    }
}
