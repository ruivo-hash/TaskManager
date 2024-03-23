using Microsoft.EntityFrameworkCore;
using TaskManager.Domain;
using TaskManager.Infra.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
var connectionString = builder.Configuration.GetConnectionString("TaskManagerConn");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/users", async (ApplicationDbContext db) =>
    await db.Users.ToListAsync());

app.MapGet("/users/{id}", async (int id, ApplicationDbContext db) =>
    await db.Users.FindAsync(id)
        is User user
            ? Results.Ok(user)
            : Results.NotFound());

app.MapPost("/users", async (User user, ApplicationDbContext db) =>
{
    db.Users.Add(user);
    await db.SaveChangesAsync();

    return Results.Created($"/users/{user.Id}", user);
});

app.MapGet("/tasks", async (ApplicationDbContext db) =>
    await db.Tasks.ToListAsync());

app.MapGet("/tasks/{id}", async (int id, ApplicationDbContext db) =>
    await db.Tasks.FindAsync(id)
        is Tasks tasks
            ? Results.Ok(tasks)
            : Results.NotFound());

app.MapPost("/tasks", async (Tasks tasks, ApplicationDbContext db) =>
{
    db.Tasks.Add(tasks);
    await db.SaveChangesAsync();

    return Results.Created($"/users/{tasks.Id}", tasks);
});

app.MapPut("/tasks/{id}", async (int id, Tasks inputTask, ApplicationDbContext db) =>
{
    var task = await db.Tasks.FindAsync(id);

    if (task is null) return Results.NotFound();

    task.Title = inputTask.Title;
    task.Description = inputTask.Description;
    task.FinishDt = inputTask.FinishDt;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/tasks/{id}", async (int id, ApplicationDbContext db) =>
{
    if (await db.Tasks.FindAsync(id) is Tasks task)
    {
        db.Tasks.Remove(task);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();

