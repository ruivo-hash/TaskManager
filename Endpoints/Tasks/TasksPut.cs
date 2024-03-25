using Microsoft.AspNetCore.Mvc;
using TaskManager.Infra.Data;

namespace TaskManager.Endpoints.Tasks;

public class TasksPut
{
    public static string Template => "/tasks/{id}";
    public static string[] Methods => new string[] { HttpMethod.Put.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action([FromRoute] int id, TasksRequest taskRequest, ApplicationDbContext context)
    {
        var task = context.Tasks.Where(t => t.Id == id).FirstOrDefault();
        task.Title = taskRequest.Title;
        task.Description = taskRequest.Description;
        task.FinishDt = taskRequest.FinishDt;

        context.SaveChanges();

        return Results.Ok();
    }
}
