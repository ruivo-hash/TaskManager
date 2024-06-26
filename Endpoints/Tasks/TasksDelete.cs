﻿using Microsoft.AspNetCore.Mvc;
using TaskManager.Infra.Data;
using TaskManager.Services;

namespace TaskManager.Endpoints.Tasks;

public class TasksDelete
{
    public static string Template => "/tasks/{id}";
    public static string[] Methods => new string[] { HttpMethod.Delete.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action([FromRoute] int id, ApplicationDbContext context, HttpContext http)
    {
        var email = TokenService.DecodingJWTtoGetEmail(http);
        var user = context.Users.Where(u => u.Email == email).FirstOrDefault();

        var task = context.Tasks.Where(t => t.Id == id).FirstOrDefault();

        if (task == null)
            return Results.NotFound();

        if (task.UserId != user.Id)
            return Results.BadRequest("Esta task não pertence a você");

        context.Tasks.Remove(task);

        context.SaveChanges();

        return Results.Ok();
    }
}
