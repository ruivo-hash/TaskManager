using TaskManager.Domain;
using TaskManager.Infra.Data;
using TaskManager.Services;

namespace TaskManager.Endpoints.Tasks;

public class TasksPost
{
    public static string Template => "/tasks";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(TasksRequest taskRequest, ApplicationDbContext context, HttpContext http)
    {
        var email = TokenService.DecodingJWTtoGetEmail(http);

        var user = context.Users.Where(u => u.Email == email).FirstOrDefault();

        var task = new Domain.Tasks(taskRequest.Title, taskRequest.Description, DateTime.UtcNow, Convert.ToDateTime(null), user.Id);

        if (!task.IsValid)
            return Results.BadRequest(task.Notifications);

        context.Tasks.Add(task);
        context.SaveChanges();

        return Results.Created($"/tasks/{task.Id}", task);
    }
}
