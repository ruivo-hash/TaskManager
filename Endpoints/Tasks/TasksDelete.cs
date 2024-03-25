using Microsoft.AspNetCore.Mvc;
using TaskManager.Infra.Data;

namespace TaskManager.Endpoints.Tasks;

public class TasksDelete
{
    public static string Template => "/tasks/{id}";
    public static string[] Methods => new string[] { HttpMethod.Delete.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action([FromRoute] int id, ApplicationDbContext context)
    {
        var task = context.Tasks.Where(t => t.Id == id).FirstOrDefault();
        context.Tasks.Remove(task);

        context.SaveChanges();

        return Results.Ok();
    }
}
