﻿using BeTraveling.Context;
using BeTraveling.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Configuration;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text.RegularExpressions;

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
            var currentUser = GetCurrentUser();
            post.UserId = currentUser.Id;
            post.Created = DateTime.Now;
            _context.Add(post);
            await _context.SaveChangesAsync();
            return Ok("Post was posted successfully");
        }
        [HttpPost]
        [Authorize]
        [Route("react/{id}/{reaction}")]
        public async Task<IActionResult> ReactPost(int id, string reaction)
        {
            var currentUser = GetCurrentUser();
            var reactions = new List<string> { "LIKE", "SAD", "FUNNY", "ANGRY", "LOVE" };
            if (reactions.Contains(reaction.ToUpper()))
            {
                if(_context.PostReactions.Where(r => r.UserId == currentUser.Id && r.PostId == id).FirstOrDefault() == null)
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


        [HttpGet]
        [Authorize]
        [Route("myposts")]
        public IActionResult GetMyPosts()
        {
            var currentUser = GetCurrentUser();
            var posts = _context.Posts.Where(post => post.UserId == currentUser.Id).ToList();

            var accumulator = new List<object>()
            .Select(t => new {Post = default(Post), ReactionNumber = default(int), CommentsNumber = default(int) }).ToList();

            if(posts != null)
            foreach (var post in posts)
            {
                var reactionsNo = _context.PostReactions.Where(reaction => post.Id == reaction.PostId).Count();
                var commentsNo = _context.Comments.Where(comment => post.Id == comment.PostId).Count();

                var result = new {Post=post,  ReactionNumber = reactionsNo, CommentsNumber = commentsNo};
                accumulator.Add(result);

            }

            return Ok(accumulator);
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
