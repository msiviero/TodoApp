using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TodoApp.Models;

public class AppContext(DbContextOptions<AppContext> options) : DbContext(options)
{
    public virtual DbSet<TodoItem> TodoItems { get; set; } = null!;

    protected override void OnConfiguring
        (DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(databaseName: "TodoDB");
    }
}

public record class TodoItem
{
    public static TodoItem Create(string key, string title, bool isCompleted) => new()
    {
        Key = key,
        Title = title,
        IsCompleted = isCompleted,
    };

    [Key]
    public required string Key { get; set; }
    public required string Title { get; set; }
    public bool IsCompleted { get; set; }
}
