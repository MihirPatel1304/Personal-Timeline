using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PersonalTimelineAPI.Data;
using PersonalTimelineAPI.Models;
using Google.Apis.Auth;

namespace PersonalTimelineAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly TimelineDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(TimelineDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("google")]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
    {
        try
        {
            // Verify Google token
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.Credential, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _configuration["Google:ClientId"] }
            });

            // Find or create user
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == payload.Email);
            
            if (user == null)
            {
                user = new User
                {
                    Email = payload.Email,
                    Name = payload.Name,
                    Picture = payload.Picture,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Update user info
                user.Name = payload.Name;
                user.Picture = payload.Picture;
                await _context.SaveChangesAsync();
            }

            // Generate JWT token
            var token = GenerateJwtToken(user);

            return Ok(new
            {
                token,
                user = new
                {
                    user.Id,
                    user.Email,
                    user.Name,
                    user.Picture
                }
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Invalid Google token", error = ex.Message });
        }
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Name, user.Name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryInMinutes"])),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public class GoogleLoginRequest
{
    public string Credential { get; set; } = string.Empty;
}
