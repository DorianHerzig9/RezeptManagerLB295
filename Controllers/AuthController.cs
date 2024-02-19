using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Rezeptmanager.Dtos;
using Rezeptmanager.Entities;
using Rezeptmanager.RezeptDB;

namespace Rezeptmanager
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly RezeptDbContext _context;

        public AuthController(IConfiguration configuration, RezeptDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("register")]
        public ActionResult<User> Register(UserDto request)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            User user = new User();

            user.Username = request.Username;
            user.PasswordHash = passwordHash;

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost("login")]
        public ActionResult<User> Login(UserDto request)
        {
            List<User> users = _context.Users.ToList();

            var user = users.SingleOrDefault(a => a.Username == request.Username);

            if (user == null)
            {
                return BadRequest("Benutzer nicht gefunden.");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return BadRequest("Falsches Passwort.");
            }

            return Ok(GenerateToken(user.Username));
        }

        static string GenerateToken(string user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new byte[256]; // Your secret key, this is just an example
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, user),
                    // Add more claims if needed
                }),
                Expires = DateTime.UtcNow.AddMinutes(2), // Token expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
