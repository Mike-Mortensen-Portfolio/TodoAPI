using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using TodoAPI.Auth0;
using TodoAPI.Context;

// Add services to the container.
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddCors();

var domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = domain;
    options.Audience = builder.Configuration["Auth0:Audience"];

    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = ClaimTypes.NameIdentifier
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ReadPolicy", policy => policy.RequireAuthenticatedUser().RequireClaim("permissions", "todo:read"));
    options.AddPolicy("WritePolicy", policy => policy.RequireAuthenticatedUser().RequireClaim("permissions", "todo:write"));
});

builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/", async () =>
{
    /*Seeding Start*/
    var scope = app.Services.CreateScope();
    using var context = scope.ServiceProvider.GetRequiredService<TodoContext>();
    await context.InsertSeed();
    /*Seeding End*/

    //  End point data
    return "Hello, World!";
});

app.MapGet("/todoitems", async (TodoContext context) =>
    await context.Todos
    .Where(i => !i.IsComplete && !i.IsRemoved)
    .ToListAsync()).RequireAuthorization();

app.MapGet("/todoitems/all", async (TodoContext context) =>
    await context.Todos
    .Where(i => !i.IsRemoved)
    .ToListAsync()).RequireAuthorization("ReadPolicy");

app.MapGet("/todoitems/{id}", async (int id, TodoContext context) =>
    await context.Todos.FindAsync(id)
        is Todo todo
            ? Results.Ok(todo)
            : Results.NotFound()
    ).RequireAuthorization("ReadPolicy");

app.MapPost("/todoitems", async (Todo todo, TodoContext context) =>
{
    if (todo.Id < 1 || todo.CreatedTime > DateTime.Now) { return Results.BadRequest($"One or more properties are invalid; {todo}"); }

    await context.AddAsync(todo);

    if (await context.SaveChangesAsync() <= 0) { return Results.BadRequest(); }

    return Results.Created($"/todoitems/{todo.Id}", todo);
}).RequireAuthorization("WritePolicy");

app.MapPut("/todoitems/{id}", async (int id, Todo input, TodoContext context) =>
{
    var todo = await context.Todos.FindAsync(id);

    if (todo == null) return Results.NotFound();    //  In case the item isn't there

    todo.Description = input.Description;
    //  We are not mapping 'todo.CreatedTime' as the property should be set when the item is created
    todo.IsComplete = input.IsComplete;
    todo.Priority = input.Priority;

    await context.SaveChangesAsync();

    return Results.NoContent();
}).RequireAuthorization("ReadPolicy", "WritePolicy");

app.MapDelete("/todoitems/{id}", async (int id, TodoContext context) =>
{
    var todo = await context.Todos.FindAsync(id);

    if (todo == null) return Results.NotFound();

    context.Remove(todo);

    await context.SaveChangesAsync();

    return Results.Ok(todo);
}).RequireAuthorization("ReadPolicy", "WritePolicy");

app.MapDelete("/todoitems/soft/{id}", async (int id, TodoContext context) =>
{
    var todo = await context.Todos.FindAsync(id);

    if (todo == null) return Results.NotFound();

    todo.IsRemoved = true;

    await context.SaveChangesAsync();

    return Results.Ok(todo);
}).RequireAuthorization("ReadPolicy", "WritePolicy");

app.UseCors(options =>
{
    options.AllowAnyOrigin();
    options.AllowAnyHeader();
    options.AllowAnyMethod();
});

app.UseAuthentication();
app.UseAuthorization();

app.Run();

public partial class Program { }