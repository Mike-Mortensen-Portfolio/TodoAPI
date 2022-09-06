using Microsoft.EntityFrameworkCore;

namespace TodoAPI.Context
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }

        public DbSet<Todo> Todos => Set<Todo>();

        internal async Task InsertSeed()
        {
            await AddRangeAsync(
                    new Todo { CreatedTime = DateTime.Now, Description = "New todo (Low)", Id = 1, IsComplete = false, Priority = Priority.Low },
                    new Todo { CreatedTime = DateTime.Now.AddDays(-1), Description = "New todo (Normal)", Id = 2, IsComplete = false, Priority = Priority.Normal },
                    new Todo { CreatedTime = DateTime.Now.AddDays(-2), Description = "New todo (High)", Id = 3, IsComplete = true, Priority = Priority.High });

            await SaveChangesAsync();
        }
    }
}
