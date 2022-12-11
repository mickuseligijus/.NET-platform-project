using BeTraveling.Context;
using BeTraveling.Models;
using BeTraveling.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BeTraveling.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly BeTravelingDbContext _context;
        private readonly IConfiguration _configuration;
        public LoginController(BeTravelingDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult UserLogin([FromBody] User user)
        {
            try
            {
                string password = Password.HashPassword(user.Password);
                var dbUser = _context.Users.Where(u => u.UserName == user.UserName && u.Password == password).FirstOrDefault();

                if (dbUser == null)
                {
                    return BadRequest("Username or password is incorrect");
                }
                if(dbUser.Status == 2)
                {
                    return BadRequest("User has been banned!");
                }
                List<Claim> autClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, dbUser.UserName),
                    new Claim(ClaimTypes.NameIdentifier, dbUser.Id.ToString()),
                    new Claim(ClaimTypes.Role, dbUser.Role)
                };
                
                var token = getToken(autClaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    creation = token.ValidFrom,
                    expiration = token.ValidTo
                });
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
                _context.Add(user);

                await _context.SaveChangesAsync();
                return Ok("User successfully registered");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        private JwtSecurityToken getToken(List<Claim> authClaim)
        {
            var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(24),
                claims: authClaim,
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }

    }
}
