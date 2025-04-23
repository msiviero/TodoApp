using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TodoApp.Models;

public class AppContext(DbContextOptions<AppContext> options) : DbContext(options)
{
    public virtual DbSet<TodoItem> TodoItems { get; set; } = null!;
}

public class TodoItem : IEquatable<TodoItem>
{
    [Key]
    public long Id { get; set; }

    [Required]
    public required string Title { get; set; }

    public bool IsCompleted { get; set; }

    public bool Equals(TodoItem? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (GetType() != other.GetType())
        {
            return false;
        }
        return Id == other.Id;
    }
}
