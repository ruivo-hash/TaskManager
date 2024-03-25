using TaskManager.Domain;
using TaskManager.Infra.Data;

namespace TaskManager.Endpoints.Tasks;

public class TasksPost
{
    public static string Template => "/tasks";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(TasksRequest taskRequest, ApplicationDbContext context)
    {
        var task = new Domain.Tasks(taskRequest.Title, taskRequest.Description, DateTime.Now, Convert.ToDateTime(null), taskRequest.UserId);

        if (!task.IsValid)
            return Results.BadRequest(task.Notifications);

        context.Tasks.Add(task);
        context.SaveChanges();

        return Results.Created($"/tasks/{task.Id}", task);
    }
}
