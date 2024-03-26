using Microsoft.IdentityModel.Tokens;
using TaskManager.Infra.Data;

namespace TaskManager.Endpoints.Tasks;

public class TasksGetByUser
{
    public static string Template => "/tasks";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(ApplicationDbContext context, HttpContext http)
    {
        var email = http.User.Claims.First().Value;
        var user = context.Users.Where(u => u.Email == email).FirstOrDefault();

        var tasks = context.Tasks.Where(t => t.UserId == user.Id);
        var response = tasks.Select(t => new TasksResponse { Id = t.Id, Title = t.Title, Description = t.Description, UserId = t.UserId, CreateDt = t.CreateDt, FinishDt = t.FinishDt });

        return Results.Ok(response);
    }
}
