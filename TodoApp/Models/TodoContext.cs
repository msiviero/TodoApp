using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TodoApp.Models;

public class AppContext(DbContextOptions<AppContext> options) : DbContext(options)
{
    public virtual DbSet<TodoItem> TodoItems { get; set; } = null!;
}

public record TodoItem
{
    [Key]
    public long Id { get; set; }

    [Required]
    public required string Title { get; set; }

    public bool IsCompleted { get; set; }
}
