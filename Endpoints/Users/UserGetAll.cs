using System.ComponentModel.DataAnnotations;
using TaskManager.Domain;
using TaskManager.Infra.Data;

namespace TaskManager.Endpoints.Users;

public class UserGetAll
{
    public static string Template => "/users";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(ApplicationDbContext context)
    {
        var users = context.Users.ToList();
        var response = users.Select(u => new UserResponse { Id = u.Id, Name = u.Name, Email = u.Email });

        return Results.Ok(response);
    }
}
