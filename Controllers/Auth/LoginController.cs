using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using imapsbackend.Data;
using imapsbackend.DTOs;
using imapsbackend.Models;

namespace imapsbackend.Controllers;

[ApiController]
[Route("api/auth/login")]
public class LoginController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public LoginController(AppDbContext context, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public class LoginResponse
    {
        public string RC { get; set; }
        public string RD { get; set; }
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null)
        {
            return Unauthorized(new LoginResponse
            {
                RC = "IV",
                RD = "Invalid email or password."
            });
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            return Unauthorized(new LoginResponse
            {
                RC = "IV",
                RD = "Invalid email or password."
            });
        }

        return Ok(new LoginResponse
        {
            RC = "00",
            RD = "Login successful."
        });
    }
}
