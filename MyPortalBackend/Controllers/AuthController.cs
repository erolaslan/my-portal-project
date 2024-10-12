using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    // JWT için gizli anahtar
    private const string SecretKey = "your_very_Strong_secret_key_96512345641254!";
    private readonly SymmetricSecurityKey _signingKey = new(Encoding.ASCII.GetBytes(SecretKey));

    [HttpPost("login")]
    public IActionResult Login([FromBody] UserLoginDto user)
    {
        // Mock kullanıcı adı ve şifre kontrolü
        if (user.Username == "admin" && user.Password == "password")
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, "Admin")
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });
        }

        return Unauthorized("Invalid username or password");
    }
}

public class UserLoginDto
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}
