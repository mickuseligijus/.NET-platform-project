using BeTraveling.Context;
using BeTraveling.Models;
using BeTraveling.Tools;
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
            try
            {
                string password = Password.HashPassword(user.Password);
                var dbUser = _context.Users.Where(u => u.UserName == user.UserName && u.Password == password).FirstOrDefault();

                if (dbUser == null)
                {
                    return BadRequest("Username or password is incorrect");
                }
                return Ok("You are loged in");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> UserRegistration([FromBody] User user)
        {

            try
            {
                var dbUser = _context.Users.Where(u => u.UserName == user.UserName).FirstOrDefault();
                if (dbUser != null)
                {
                    return BadRequest("Username already exists");
                }

                user.Password = Password.HashPassword(user.Password);
                user.Status = 1;
                user.CreatedDate = DateTime.Now;
                _context.Add(user);

                await _context.SaveChangesAsync();
                return Ok("User successfully registered");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

    }
}
