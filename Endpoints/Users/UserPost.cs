﻿using TaskManager.Domain;
using TaskManager.Infra.Data;

namespace TaskManager.Endpoints.Users;

public class UserPost
{
    public static string Template => "/users";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(UserRequest userRequest, ApplicationDbContext context)
    {
        var user = new User(userRequest.Name, userRequest.Email, userRequest.Password);

        if (!user.IsValid)
            return Results.BadRequest(user.Notifications);

        var userCreated = context.Users.Where(u => u.Email == user.Email).FirstOrDefault();
        if (userCreated != null)
            return Results.BadRequest("E-mail já cadastrado no sistema");

        context.Users.Add(user);
        context.SaveChanges();

        return Results.Created($"/users/{user.Id}", user);
    }
}
