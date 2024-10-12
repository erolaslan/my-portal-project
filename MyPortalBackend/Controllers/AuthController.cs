using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyPortalBackend.Data;
using MyPortalBackend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly SymmetricSecurityKey _signingKey;

    public AuthController(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        var secretKey = configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("JWT secret key not found.");
        _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] UserRegisterDto userDto)
    {
        if (userDto == null || string.IsNullOrEmpty(userDto.Username) || string.IsNullOrEmpty(userDto.Password))
            return BadRequest("Invalid registration request.");

        if (_context.Users.Any(u => u.Username == userDto.Username))
            return BadRequest("Username already exists.");

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

        var newUser = new User
        {
            Username = userDto.Username,
            PasswordHash = hashedPassword
        };

        _context.Users.Add(newUser);
        _context.SaveChanges();

        return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] UserLoginDto userDto)
    {
        if (userDto == null || string.IsNullOrEmpty(userDto.Username) || string.IsNullOrEmpty(userDto.Password))
            return BadRequest("Invalid login request.");

        var user = _context.Users.SingleOrDefault(u => u.Username == userDto.Username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
            return Unauthorized("Invalid username or password.");

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "User")
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(new { Token = tokenString });
    }
}

public class UserLoginDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class UserRegisterDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
