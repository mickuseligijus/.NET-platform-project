using BeTraveling.Context;
using BeTraveling.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
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
        [Authorize]
        [Route("post")]
        public async Task<IActionResult> SharePost([FromBody] Post post)
        {
            try
            {
                var currentUser = GetCurrentUser();
                post.UserId = currentUser.Id;
                post.Created = DateTime.Now;
                _context.Add(post);
                await _context.SaveChangesAsync();
                return Ok("Post was posted successfully");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        [HttpGet]
        [Authorize]
        [Route("reactions/{id}")]
        public async Task<IActionResult> GetReactions(int id)
        {
            var reactions = _context.PostReactions.Where(reaction => reaction.PostId == id).ToList();
            var result = new List<object>()
                .Select(t => new
                {
                    Reaction = default(ReactionPost),
                    UserName = default(string)
                }).ToList();
            foreach (var reaction in reactions)
            {
                var userInfo = _context.Users.SingleOrDefault(user => user.Id == reaction.UserId);
                var r = new { Reaction = reaction, UserName = userInfo.UserName };

                result.Add(r);
            }



            return Ok(result);
        }
        [HttpPost]
        [Authorize]
        [Route("react/{id}/{reaction}")]
        public async Task<IActionResult> ReactPost(int id, string reaction)
        {
            try
            {
                var currentUser = GetCurrentUser();
                var reactions = new List<string> { "LIKE", "SAD", "FUNNY", "ANGRY", "LOVE" };
                if (reactions.Contains(reaction.ToUpper()))
                {
                    if (_context.PostReactions.Where(r => r.UserId == currentUser.Id && r.PostId == id).FirstOrDefault() == null)
                    {
                        _context.PostReactions.Add(new ReactionPost { ReactionType = reaction.ToUpper(), PostId = id, UserId = currentUser.Id });
                        await _context.SaveChangesAsync();
                        return Ok("Reaction saved");
                    }
                    else if (_context.PostReactions.Where(r => r.UserId == currentUser.Id && r.PostId == id && r.ReactionType != reaction.ToUpper()).FirstOrDefault() != null)
                    {
                        var postReaction = _context.PostReactions.Where(r => r.UserId == currentUser.Id && r.PostId == id && r.ReactionType != reaction.ToUpper()).FirstOrDefault();
                        postReaction.ReactionType = reaction.ToUpper();
                        await _context.SaveChangesAsync();
                        return Ok("Reaction changed");
                    }
                    else if (_context.PostReactions.Where(r => r.UserId == currentUser.Id && r.PostId == id && r.ReactionType == reaction.ToUpper()).FirstOrDefault() != null)
                    {
                        var postReaction = _context.PostReactions.Where(r => r.UserId == currentUser.Id && r.PostId == id && r.ReactionType == reaction.ToUpper()).FirstOrDefault();
                        _context.PostReactions.Remove(postReaction);
                        await _context.SaveChangesAsync();
                        return Ok("Reaction removed");
                    }
                }

                return BadRequest("Incorrect reaction type");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpGet]
        [Authorize]
        [Route("friendsPosts")]
        public IActionResult GetFriendsPosts()
        {
            try
            {
                var currentUser = GetCurrentUser();
                var sqlParameter = new SqlParameter("@UserId", currentUser.Id);

                var posts = _context.Posts.FromSqlInterpolated($"SELECT * FROM posts WHERE (posts.UserId IN ((SELECT userId2 from friends Where UserId1 = {sqlParameter} AND Status = 0 ) Union (SELECT userId1 from friends Where UserId2 = {sqlParameter} AND Status = 0)) OR posts.UserId = {sqlParameter}) AND posts.IsDeleted = 'False'").OrderByDescending(post => post.Created).ToList(); //EXCEPT (SELECT * from posts where UserId = {sqlParameter})").ToList();

                var accumulator = new List<object>()
                .Select(t => new { Post = default(Post), ReactionNumber = default(int), CommentsNumber = default(int), UserInfo = default(string) }).ToList();

                if (posts != null)
                    foreach (var post in posts)
                    {
                        var reactionsNo = _context.PostReactions.Where(reaction => post.Id == reaction.PostId).Count();
                        var commentsNo = _context.Comments.Where(comment => post.Id == comment.PostId).Count();
                        var userInfo = _context.Users.SingleOrDefault(user => user.Id == post.UserId);

                        var result = new { Post = post, ReactionNumber = reactionsNo, CommentsNumber = commentsNo, UserInfo = userInfo.UserName  };
                        accumulator.Add(result);

                    }

                return Ok(accumulator);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpGet]
        [Authorize]
        [Route("myposts")]
        public IActionResult GetMyPosts()
        {
            try
            {
                var currentUser = GetCurrentUser();
                var posts = _context.Posts.Where(post => post.UserId == currentUser.Id && post.IsDeleted==false).OrderByDescending(post => post.Created).ToList();

                /*      var accumulator = new List<object>()
                      .Select(t => new { Post = default(Post), ReactionNumber = default(int), CommentsNumber = default(int) }).ToList();*/
                var accumulator = new List<object>()
            .Select(t => new { Post = default(Post), ReactionNumber = default(int), CommentsNumber = default(int), UserInfo = default(string) }).ToList();

                if (posts != null)
                    foreach (var post in posts)
                    {
                        var reactionsNo = _context.PostReactions.Where(reaction => post.Id == reaction.PostId).Count();
                        var commentsNo = _context.Comments.Where(comment => post.Id == comment.PostId).Count();
                       /* var userInfo = _context.Users.SingleOrDefault(user => user.Id == post.UserId);*/

                        var result = new { Post = post, ReactionNumber = reactionsNo, CommentsNumber = commentsNo, UserInfo = currentUser.UserName };

                       /* var result = new { Post = post, ReactionNumber = reactionsNo, CommentsNumber = commentsNo };*/
                        accumulator.Add(result);

                    }

                return Ok(accumulator);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpPost]
        [Authorize]
        [Route("remove/{id}")]
        public async Task<IActionResult> RemovePost(int id)
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null)
            {
                return BadRequest("User was not found");
            }
            var post = _context.Posts.SingleOrDefault(post => (post.Id == id && post.UserId == currentUser.Id));
            if (post == null)
            {
                return BadRequest("There is no such post");
            }
            post.IsDeleted = true;
            await _context.SaveChangesAsync();

            return Ok("Post has been removed");
        }

        [HttpPost]
        [Authorize]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] Post post)
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null)
            {
                return BadRequest("User was not found");
            }
            var postOld = _context.Posts.SingleOrDefault(post => (post.Id == id && post.UserId == currentUser.Id));
            if (postOld == null)
            {
                return BadRequest("There is no such post");
            }
            if (!post.Text.Equals(""))
            {
                postOld.Text = post.Text;

            }
            if (!post.Title.Equals(""))
            {
                postOld.Title = post.Title;

            }
            await _context.SaveChangesAsync();
            return Ok("Post has been modified");
        }

        [HttpGet]
        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null)
            {
                return BadRequest("User was not found");
            }
            var post = _context.Posts.SingleOrDefault(post => post.Id==id && post.UserId == currentUser.Id);
            if(post == null)
            {
                return BadRequest("Post was not found");
            }
            return Ok(post);

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
