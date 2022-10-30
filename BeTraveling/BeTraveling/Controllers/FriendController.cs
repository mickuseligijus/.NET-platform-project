using BeTraveling.Context;
using BeTraveling.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BeTraveling.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendController : ControllerBase
    {
        private readonly BeTravelingDbContext _context;

        public FriendController(BeTravelingDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            try
            {
                var currentUser = GetCurrentUser();
                if (currentUser == null)
                {
                    return BadRequest("User was not found");
                }

                var friends1 =
                    from friend in _context.Friends
                    join user in _context.Users on friend.UserId2 equals user.Id
                    where friend.UserId1 == currentUser.Id && friend.Status == 0
                    select user.UserName;

                var friends2 =
                    from friend in _context.Friends
                    join user in _context.Users on friend.UserId1 equals user.Id
                    where friend.UserId2 == currentUser.Id && friend.Status == 0
                    select user.UserName;

                var friends = friends1.Union(friends2);

                return Ok(friends);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("invitations")]
        public IActionResult GetInvitations()
        {
            try
            {
                var currentUser = GetCurrentUser();
                if (currentUser == null)
                {
                    return BadRequest("User was not found");
                }

                var friends =
                    from friend in _context.Friends
                    join user in _context.Users on friend.UserId2 equals user.Id
                    where friend.UserId1 == currentUser.Id && friend.Status == 1
                    select user.UserName;

                return Ok(friends);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost]
        [Authorize]
        [Route("add")]
        public async Task<IActionResult> AddFriend([FromBody] Friend friend)
        {
            try
            {
                var currentUser = GetCurrentUser();
                if (currentUser == null)
                {
                    return BadRequest("User was not found");
                }
                var newFriend = _context.Users.FirstOrDefault(x => x.Id == friend.UserId2);
                if (newFriend == null)
                {
                    return BadRequest("Cannot add this user because there is no such user");
                }
                var friends = _context.Friends.Where(f => (f.UserId1 == currentUser.Id && f.UserId2 == friend.UserId2) || (f.UserId1 == friend.UserId2 && f.UserId2 == currentUser.Id)).FirstOrDefault();
                if (friends != null)
                {
                    if (friends.Status == 0)
                    {
                        return Ok("Users are already friends!");
                    }
                    return Ok("Invitation was already sent");
                }
                friend.UserId1 = currentUser.Id;
                friend.Status = 1;
                _context.Friends.Add(friend);
                await _context.SaveChangesAsync();

                return Ok("Friend request was sent successfuly");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
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
