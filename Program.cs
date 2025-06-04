using imapsbackend.Data;
using imapsbackend.Controllers;
using imapsbackend.Routes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<RegisterController>();
builder.Services.AddScoped<LoginController>();

builder.Services.AddScoped<IPasswordHasher<imapsbackend.Models.User>, PasswordHasher<imapsbackend.Models.User>>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Mapping routes dari eksternal class
app.MapAuthRoutes();

app.Run();
