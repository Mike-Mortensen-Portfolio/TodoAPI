using Microsoft.EntityFrameworkCore;
using TodoAPI.Context;

// Add services to the container.
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddCors();
builder.Services.AddAuthentication();

var app = builder.Build();

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapGet("/", async () =>
{
    /*Seeding Start*/
    var scope = app.Services.CreateScope();
    using var context = scope.ServiceProvider.GetRequiredService<TodoContext>();
    await context.InsertSeed();
    /*Seeding End*/

    //  End point data
    return "Hello, World!";
})/*.RequireAuthorization()*/;

app.MapGet("/todoitems", async (TodoContext context) => await context.Todos
                                                                .Where(i => !i.IsComplete && !i.IsRemoved)
                                                                .ToListAsync()
          )/*.RequireAuthorization()*/;

app.MapGet("/todoitems/all", async (TodoContext context) =>
    await context.Todos
    .Where(i => !i.IsRemoved)
    .ToListAsync());

app.MapGet("/todoitems/{id}", async (int id, TodoContext context) =>
    await context.Todos.FindAsync(id)
        is Todo todo
            ? Results.Ok(todo)
            : Results.NotFound()
    )/*.RequireAuthorization()*/;

app.MapPost("/todoitems", async (Todo todo, TodoContext context) =>
{
    if (todo.Id < 1 || todo.CreatedTime > DateTime.Now) { return Results.BadRequest($"One or more properties are invalid; {todo}"); }

    await context.AddAsync(todo);

    if (await context.SaveChangesAsync() <= 0) { return Results.BadRequest(); }

    return Results.Created($"/todoitems/{todo.Id}", todo);
})/*.RequireAuthorization()*/;

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
})/*.RequireAuthorization()*/;

app.MapDelete("/todoitems/{id}", async (int id, TodoContext context) =>
{
    var todo = await context.Todos.FindAsync(id);

    if (todo == null) return Results.NotFound();

    context.Remove(todo);

    await context.SaveChangesAsync();

    return Results.Ok(todo);
})/*.RequireAuthorization()*/;

app.MapDelete("/todoitems/soft/{id}", async (int id, TodoContext context) =>
{
    var todo = await context.Todos.FindAsync(id);

    if (todo == null) return Results.NotFound();

    todo.IsRemoved = true;

    await context.SaveChangesAsync();

    return Results.Ok(todo);
})/*.RequireAuthorization()*/;

app.UseCors();

app.Run();

public partial class Program { }