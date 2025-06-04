using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TicketManagementSystem.Models;
using TicketManagementSystem.DTOs;
using TicketManagementSystem.Data;
using Microsoft.AspNetCore.Identity;

namespace TicketManagementSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly TicketManagementDbContext _context;
    private readonly IConfiguration _config;

    public UserController(TicketManagementDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto login)
    {
        if (login == null || string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
            return BadRequest("Email and password are required.");

        var user = _context.Users.FirstOrDefault(u => u.Email == login.Email);
        if (user == null)
            return Unauthorized("Invalid credentials");

        var hasher = new PasswordHasher<User>();
        var result = hasher.VerifyHashedPassword(null, user.Password, login.Password);

        var token = GenerateJwtToken(user);
        return Ok(new { token });
    }



    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterDto register)
    {
        if (_context.Users.Any(u => u.Email == register.Email))
            return BadRequest("User already exists.");

        var user = new User
        {
            Name = register.Name,
            Email = register.Email,
            Password = new PasswordHasher<User>().HashPassword(null, register.Password),
            Role = "User"
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return Ok("User registered.");
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:ExpireMinutes"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
