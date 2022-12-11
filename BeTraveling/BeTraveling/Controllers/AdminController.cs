using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BeTraveling.Context;

namespace BeTraveling.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly BeTravelingDbContext _context;

        public AdminController(BeTravelingDbContext context)
        {
            _context = context;

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("users")]
        public IActionResult GetAll()
        {
            return Ok(_context.Users.Where(user => user.Status==1).ToList());
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("banned/users")]
        public IActionResult GetBannedUsers()
        {
            return Ok(_context.Users.Where(user => user.Status==2).ToList());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("ban/{id}")]
        public IActionResult Ban(int id)
        {
            var user = _context.Users.SingleOrDefault(x => x.Id == id);
            if (user == null)
            {
                return BadRequest("There is no such user");
            }
            user.Status = 2;

            _context.SaveChangesAsync();
            return Ok("User has been banned successfuly");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("unban/{id}")]
        public IActionResult Unban(int id)
        {
            var user = _context.Users.SingleOrDefault(x => x.Id == id);
            if (user == null)
            {
                return BadRequest("There is no such user");
            }
            user.Status = 1;

            _context.SaveChangesAsync();
            return Ok("User has been unbanned successfuly");
        }



    }
}
