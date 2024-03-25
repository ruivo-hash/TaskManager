using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(configuration["JwtBearerTokenSettings:SecretKey"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        var jwtSecurityToken = tokenHandler.ReadJwtToken(tokenHandler.WriteToken(token));
        var emailToken = jwtSecurityToken.Claims.First(c => c.Type == "email").Value;

        return Results.Ok(new
        {
            token = tokenHandler.WriteToken(token)
        });
    }
}
