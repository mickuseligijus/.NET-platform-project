using BeTraveling.Context;
using BeTraveling.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BeTraveling.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly BeTravelingDbContext _context;

        public PostController(BeTravelingDbContext context)
        {
            _context = context;
        }


        [HttpPost]
        [Route("share")]
        public async Task<IActionResult> SharePost([FromBody] Post post)
        {
            var currentUser = GetCurrentUser();
            return Ok();
        }


        private User GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;
                return new User
                {
                    UserName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Name)?.Value,
                    Id = int.Parse(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value),
                    Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value
                };
            }
            return null;
        }
    }
}
