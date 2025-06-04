using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using imapsbackend.Data;
using imapsbackend.DTOs;
using imapsbackend.Models;
using imapsbackend.Helpers; 

namespace imapsbackend.Controllers;

[ApiController]
[Route("api/auth/login")]
public class LoginController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IConfiguration _configuration;

    public LoginController(AppDbContext context, IPasswordHasher<User> passwordHasher, IConfiguration configuration)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
    }

    public class LoginResponse
    {
        public string RC { get; set; }
        public string RD { get; set; }
        public object Data { get; set; }
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null)
        {
            return Unauthorized(new LoginResponse
            {
                RC = "IV",
                RD = "Invalid email or password.",
                Data = null
            });
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            return Unauthorized(new LoginResponse
            {
                RC = "IV",
                RD = "Invalid email or password.",
                Data = null
            });
        }

        var token = JwtHelper.GenerateJwtToken(user, _configuration);

        return Ok(new LoginResponse
        {
            RC = "00",
            RD = "Login successful.",
            Data = new { Token = token }
        });
    }
}
