namespace TodoApp.Tests.Services;

using TodoApp.Models;
using TodoApp.Services;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class TodoServiceTests
{
    [Fact]
    public void ShouldFetchOneTodo()
    {
        var mockSet = new Mock<DbSet<TodoItem>>();
        var mockContext = new Mock<AppContext>(new DbContextOptions<AppContext>());

        mockSet.Setup(m => m.Find(It.Is<long>(x => x == 7))).Returns(new TodoItem { Id = 7, Title = "Buy milk", IsCompleted = false });
        mockContext.Setup(c => c.TodoItems).Returns(mockSet.Object);

        var underTest = new TodoService(mockContext.Object);

        var result = underTest.Get(7);

        Assert.NotNull(result);
        Assert.Equal(new TodoItem { Id = 7, Title = "Buy milk", IsCompleted = false }, result);

        mockSet.Verify(m => m.Find(It.Is<long>(x => x == 7)), Times.Once);
    }

    [Fact]
    public void ShouldFetchAllTodos()
    {
        var mockSet = new Mock<DbSet<TodoItem>>();
        var mockContext = new Mock<AppContext>(new DbContextOptions<AppContext>());

        List<TodoItem> items = [
            new TodoItem { Id = 7, Title = "Buy milk", IsCompleted = false },
           new TodoItem { Id = 17, Title = "Buy bread", IsCompleted = true }
        ];

        mockSet.Setup(m => m.AsQueryable()).Returns(items.AsQueryable());
        mockContext.Setup(c => c.TodoItems).Returns(mockSet.Object);

        var underTest = new TodoService(mockContext.Object);
        var result = underTest.GetAll();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void ShouldFilterTodos()
    {
        var mockSet = new Mock<DbSet<TodoItem>>();
        var mockContext = new Mock<AppContext>(new DbContextOptions<AppContext>());

        List<TodoItem> items = [
          new TodoItem { Id = 7, Title = "Buy milk", IsCompleted = false },
          new TodoItem { Id = 17, Title = "Buy bread", IsCompleted = true }
       ];

        mockSet.Setup(m => m.AsQueryable()).Returns(items.AsQueryable());
        mockContext.Setup(c => c.TodoItems).Returns(mockSet.Object);

        var underTest = new TodoService(mockContext.Object);
        var result = underTest.GetAll("milk");

        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task ShouldCreateATodo()
    {
        var mockSet = new Mock<DbSet<TodoItem>>();
        var mockContext = new Mock<AppContext>(new DbContextOptions<AppContext>());

        mockContext.Setup(c => c.TodoItems).Returns(mockSet.Object);

        var underTest = new TodoService(mockContext.Object);
        var item = new TodoItem { Id = 7, Title = "Buy milk", IsCompleted = false };

        await underTest.Create(item);

        mockSet.Verify(m => m.Add(It.Is<TodoItem>(x => x == item)), Times.Once);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ShouldEditATodo()
    {
        var item = new TodoItem { Id = 7, Title = "Buy milk", IsCompleted = false };

        var mockSet = new Mock<DbSet<TodoItem>>();
        var mockContext = new Mock<AppContext>(new DbContextOptions<AppContext>());

        mockContext.Setup(c => c.TodoItems).Returns(mockSet.Object);
        mockSet.Setup(m => m.Find(It.Is<long>(x => x == 7))).Returns(new TodoItem { Id = 7, Title = "Buy milk", IsCompleted = false });

        var underTest = new TodoService(mockContext.Object);
        await underTest.Create(item);

        var updatedItem = new TodoItem { Id = 7, Title = "Buy eggs", IsCompleted = true };
        await underTest.Edit(7, updatedItem);

        mockSet.Verify(m => m.Find(It.Is<long>(x => x == 7)), Times.Once);
        mockSet.Verify(m => m.Update(It.Is<TodoItem>(x => x.Equals(updatedItem))), Times.Once);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Fact]
    public async Task ShouldDeleteATodo()
    {
        var mockSet = new Mock<DbSet<TodoItem>>();
        var mockContext = new Mock<AppContext>(new DbContextOptions<AppContext>());

        mockContext.Setup(c => c.TodoItems).Returns(mockSet.Object);
        mockSet.Setup(m => m.Find(It.Is<long>(x => x == 7))).Returns(new TodoItem { Id = 7, Title = "Buy milk", IsCompleted = false });

        var underTest = new TodoService(mockContext.Object);
        await underTest.Delete(7);

        mockSet.Verify(m => m.Find(It.Is<long>(x => x == 7)), Times.Once);
        mockSet.Verify(m => m.Remove(It.Is<TodoItem>(x => x.Id == 7)), Times.Once);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
