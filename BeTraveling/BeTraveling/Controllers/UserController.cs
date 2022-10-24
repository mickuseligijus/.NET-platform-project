using BeTraveling.Context;
using BeTraveling.Models;
using BeTraveling.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeTraveling.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly BeTravelingDbContext _context;

        public UserController(BeTravelingDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> UserLogin([FromBody] User user)
        {
            string password = Password.HashPassword(user.Password);
            var dbUser = _context.Users.Where(u => u.UserName == user.UserName && u.Password == password).FirstOrDefault();

            if (dbUser == null)
            {
                return BadRequest("Username or password is incorrect");
            }
            return Ok("You are loged in");
        }
        public async Task<IActionResult> UserRegistration([FromBody] User user)
        {
            var dbUser = _context.Users.Where(u => u.UserName == user.UserName).FirstOrDefault();
            if(dbUser != null)
            {
                return BadRequest("Username already exists");
            }

            user.Password = Password.HashPassword(user.Password);
            _context.Add(user);

            await _context.SaveChangesAsync();
            return Ok("");
        }

    }
}
