using Microsoft.EntityFrameworkCore;

namespace TodoApp.Models;

public class AppContext(DbContextOptions<AppContext> options) : DbContext(options)
{
    public virtual DbSet<TodoItem> TodoItems { get; set; } = null!;
}

public record TodoItem(
    long Id, 
    string Title,
    bool IsCompleted
);
