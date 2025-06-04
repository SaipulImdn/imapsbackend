using Microsoft.AspNetCore.Builder;
using imapsbackend.Controllers;
using imapsbackend.DTOs;

namespace imapsbackend.Routes;

public static class AuthRoutes
{
    public static void MapAuthRoutes(this WebApplication app)
    {
        app.MapPost("/api/v1/register", async (RegisterDto dto, RegisterController controller) =>
        {
            var result = await controller.Register(dto);
            return result switch
            {
                Microsoft.AspNetCore.Mvc.OkObjectResult ok => Results.Ok(ok.Value),
                Microsoft.AspNetCore.Mvc.BadRequestObjectResult bad => Results.BadRequest(bad.Value),
                _ => Results.Problem()
            };
        });

        app.MapPost("/api/v1/login", async (LoginDto dto, LoginController controller) =>
        {
            var result = await controller.Login(dto);
            return result switch
            {
                Microsoft.AspNetCore.Mvc.OkObjectResult ok => Results.Ok(ok.Value),
                Microsoft.AspNetCore.Mvc.UnauthorizedObjectResult unauthorized => Results.Unauthorized(),
                _ => Results.Problem()
            };
        });
    }
}
