using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TodoApp.Models;

public class TodoAppContext(DbContextOptions<TodoAppContext> options) : DbContext(options)
{
    public virtual DbSet<Todo> TodoItems { get; set; } = null!;
}

[Table("todo")]
public record class Todo
{
    public static Todo Create(
        string key,
        DateTime CreatedAt,
        DateTime LastModifiedAt,
        string title,
        bool isCompleted
        ) => new()
        {
            Key = key,
            Title = title,
            IsCompleted = isCompleted,
            CreatedAt = CreatedAt,
            LastModifiedAt = LastModifiedAt,
        };

    [Key]
    public required string Key { get; set; }
    public required string Title { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastModifiedAt { get; set; }
}
