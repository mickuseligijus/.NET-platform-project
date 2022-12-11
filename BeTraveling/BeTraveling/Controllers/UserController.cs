using BeTraveling.Context;
using BeTraveling.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Route("info")]
  /*      public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public bool HideLocation { get; set; }
        public string Location { get; set; }
        public bool IsDeleted { get; set; } = false;
        [ForeignKey("User")]
        public int UserId { get; set; }*/
        public IActionResult GetInfo()
        {
            var currentUser = GetCurrentUser(); 
            var userInfo = _context.UsersInfo.SingleOrDefault(user => user.UserId == currentUser.Id);

            if(userInfo != null)
            {
                var info = new { Id = userInfo.Id, Name = userInfo.Name, LastName = userInfo.LastName, HideLocation = userInfo.HideLocation, Location = userInfo.Location };

                return Ok(info);
            }
            return Ok(userInfo);
        }

        [HttpGet]
        [Authorize]
        [Route("people")]
        public IActionResult Get()
        {
            var currentUser = GetCurrentUser();
            var availableUsers = _context.Users.Where(user => user.Id!= currentUser.Id && user.Role.Equals("Traveler") && user.Status==1).Select(user => new { user.UserName, user.Id }).ToList();

            var result = new List<object>()
            .Select(t => new
            {
                UserId = default(int),
                UserName = default(string),
                Friendship = default(string)
            }).ToList();

            foreach (var user in availableUsers)
            {
               var friendship = _context.Friends.FirstOrDefault(friend => (friend.UserId1 == currentUser.Id && friend.UserId2 == user.Id) || (friend.UserId2 == currentUser.Id && friend.UserId1 == user.Id));
                if (friendship != null)
                {

                }
                else
                {
                    result.Add(new { UserId = user.Id, UserName = user.UserName, Friendship = "Strangers" });
                }
            }
            return Ok(result);
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
