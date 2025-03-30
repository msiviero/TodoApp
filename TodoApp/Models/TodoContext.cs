using Microsoft.EntityFrameworkCore;

namespace TodoApp.Models;

public class TodoContext(DbContextOptions<TodoContext> options) : DbContext(options)
{
    public virtual DbSet<TodoItem> TodoItems { get; set; } = null!;
}

public record TodoItem(long Id, string Title, bool IsCompleted);
