using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManager.Domain;
using TaskManager.Endpoints.Login;
using TaskManager.Endpoints.Tasks;
using TaskManager.Endpoints.Users;
using TaskManager.Infra.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("TaskManagerConn");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtBearerTokenSettings:SecretKey"]);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateAudience = false,
        ValidateIssuer = false
    };
});
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapMethods(TokenPost.Template, TokenPost.Methods, TokenPost.Handle);

app.MapMethods(UserPost.Template, UserPost.Methods, UserPost.Handle);

app.MapMethods(UserGetAll.Template, UserGetAll.Methods, UserGetAll.Handle);

app.MapMethods(TasksGetByUser.Template, TasksGetByUser.Methods, TasksGetByUser.Handle).RequireAuthorization();

app.MapMethods(TasksGetById.Template, TasksGetById.Methods, TasksGetById.Handle).RequireAuthorization();

app.MapMethods(TasksPost.Template, TasksPost.Methods, TasksPost.Handle);

app.MapMethods(TasksPut.Template, TasksPut.Methods, TasksPut.Handle).RequireAuthorization();

app.MapMethods(TasksDelete.Template, TasksDelete.Methods, TasksDelete.Handle).RequireAuthorization();

app.Run();

