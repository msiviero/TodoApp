using Microsoft.EntityFrameworkCore;
using TodoApp.Services;

namespace TodoApp.Models;

public class TodoContext(DbContextOptions<TodoContext> options) : DbContext(options)
{
    public DbSet<TodoItem> TodoItems { get; set; } = null!;
}
