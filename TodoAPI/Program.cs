using Microsoft.EntityFrameworkCore;
using TodoAPI.Context;

// Add services to the container.
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/", async () =>
{
    using var context = app.Services.GetRequiredService<TodoContext>();
    await context.InsertSeed();

    return "Hello, World!";
});

app.MapGet("/todoitems", async (TodoContext context) => await context.Todos.ToListAsync());

app.MapGet("/todoitems/all", async (TodoContext context) =>
    await context.Todos
    .Where(i => !i.IsComplete)
    .ToListAsync());

app.MapGet("/todoitems/{id}", async (int id, TodoContext context) =>
    await context.Todos.FindAsync(id)
        is Todo todo
            ? Results.Ok(todo)
            : Results.NotFound());

app.MapPost("/todoitems", async (Todo todo, TodoContext context) =>
{
    await context.AddAsync(todo);

    await context.SaveChangesAsync();

    return Results.Created($"/todoitems/{todo.Id}", todo);
});

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
});

app.MapDelete("/todoitems/{id}", async (int id, TodoContext context) =>
{
    var todo = await context.Todos.FindAsync(id);

    if (todo == null) return Results.NotFound();

    context.Remove(todo);

    await context.SaveChangesAsync();

    return Results.Ok(todo);
});

app.Run();

public partial class Program { }