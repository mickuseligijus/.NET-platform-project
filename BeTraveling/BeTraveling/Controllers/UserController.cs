using BeTraveling.Context;
using BeTraveling.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [HttpGet]
        [Authorize]
        [Route("people")]
        public IActionResult Get()
        {
            var currentUser = GetCurrentUser();
            var availableUsers = _context.Users.Where(user => user.Id!= currentUser.Id && user.Role.Equals("Traveler")).Select(user => new { user.UserName, user.Id }).ToList();
            return Ok(availableUsers);
        }
        [HttpPost]
        [Authorize]
        [Route("info")]
        public async Task<IActionResult> UpdateInfo([FromBody] UserInfo userInfo)
        {
            try
            {
                var currentUser = GetCurrentUser();
                if (currentUser == null)
                {
                    return BadRequest("User was not found");
                }
                var userInfoInstance = _context.UsersInfo.Where(u => u.UserId == currentUser.Id).FirstOrDefault();
                if(userInfoInstance == null)
                {
                    userInfo.UserId = currentUser.Id;
                    _context.Add(userInfo);
                    await _context.SaveChangesAsync();
                    return Ok("Information was added successfuly");

                }
                userInfoInstance.Name = userInfo.Name;
                userInfoInstance.LastName = userInfo.LastName;
                userInfoInstance.Location = userInfo.Location;
                userInfoInstance.HideLocation = userInfo.HideLocation;
                await _context.SaveChangesAsync();

                return Ok("Information was updated successfuly");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }
        private User GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if(identity !=null)
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
