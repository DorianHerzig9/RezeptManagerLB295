using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Rezeptmanager.Dtos;
using Rezeptmanager.Entities;
using Rezeptmanager.RezeptDB;
using System.IdentityModel.Tokens.Jwt;

namespace Rezeptmanager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RezeptController : ControllerBase
    {
        private readonly RezeptDbContext _context;
        public RezeptController(RezeptDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddRezept(RezeptDto rezept, string jwtToken)
        {
            if (IsTokenValid(jwtToken))
            {
                await _context.Rezepte.AddAsync(RezeptMapper.Map(rezept));
                await _context.SaveChangesAsync();

                return Ok();
            }
            else
            {
                return BadRequest("Falsches JWT Token.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rezept>>> GetRezepte(string jwtToken)
        {
            if (IsTokenValid(jwtToken))
            {
                return _context.Rezepte.Include(r => r.Zutaten).ToList();
            }
            else
            {
                return BadRequest("Falsches JWT Token.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Rezept>> GetRezept(int id, string jwtToken)
        {
            if (IsTokenValid(jwtToken))
            {
                return _context.Rezepte.Include(r => r.Zutaten).SingleOrDefault(x => x.Id == id);
            }
            else
            {
                return BadRequest("Falsches JWT Token.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRezept(int id, RezeptDto rezept, string jwtToken)
        {
            if (IsTokenValid(jwtToken))
            {
                var existingRezept = _context.Rezepte.Include(r => r.Zutaten).AsTracking().FirstOrDefault(x => x.Id == id);
                _context.Rezepte.Remove(existingRezept);
                _context.Rezepte.Add(RezeptMapper.Map(rezept));
                await _context.SaveChangesAsync();

                return Ok();
            }
            else
            {
                return BadRequest("Falsches JWT Token.");
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRezept(int id, string jwtToken)
        {
            if (IsTokenValid(jwtToken))
            {
                var rezept = _context.Rezepte.Include(r => r.Zutaten).AsTracking().FirstOrDefault(x => x.Id == id);

                _context.Rezepte.Remove(rezept);
                await _context.SaveChangesAsync();

                return Ok();
            }
            else
            {
                return BadRequest("Falsches JWT Token.");
            }
        }

        static bool IsTokenValid(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(new byte[256]), // Your secret key, this is just an example
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.FromMinutes(5) // Adjusted ClockSkew value
                }, out SecurityToken validatedToken);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
