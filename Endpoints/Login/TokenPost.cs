using TaskManager.Services;
using TaskManager.Infra.Data;

namespace TaskManager.Endpoints.Login;

public class TokenPost
{
    public static string Template => "/token";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(LoginRequest request, ApplicationDbContext context, IConfiguration configuration)
    {
        var user = context.Users.Where(u => u.Email == request.Email).FirstOrDefault();

        if (user.Password != request.Password)
            Results.BadRequest();

        var token = TokenService.GenerateToken(configuration, user);

        //var jwtSecurityToken = tokenHandler.ReadJwtToken(tokenHandler.WriteToken(token));
        //var emailToken = jwtSecurityToken.Claims.First(c => c.Type == "email").Value;

        return Results.Ok(token);
    }
}
