using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using imapsbackend.Data;
using imapsbackend.DTOs;
using imapsbackend.Models;

namespace imapsbackend.Controllers;

[ApiController]
[Route("api/auth/register")]
public class RegisterController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public RegisterController(AppDbContext context, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public class RegisterResponse
    {
        public string RC { get; set; }
        public string RD { get; set; }
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
        {
            return BadRequest(new RegisterResponse
            {
                RC = "EX",
                RD = "Email already in use."
            });
        }

     var user = new User
    {
        Id = Guid.NewGuid(),
        Username = dto.Username,
        Email = dto.Email,
        BirthDate = DateTime.SpecifyKind(dto.BirthDate, DateTimeKind.Utc),
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    };

        user.Password = _passwordHasher.HashPassword(user, dto.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new RegisterResponse
        {
            RC = "00",
            RD = "User registered successfully."
        });
    }
}
