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

        mockSet.Setup(m => m.Find(It.Is<string>(x => x == "abc"))).Returns(new TodoItem("abc", "Buy milk", false));
        mockContext.Setup(c => c.TodoItems).Returns(mockSet.Object);

        var underTest = new TodoService(mockContext.Object);

        var result = underTest.Get("abc");

        Assert.NotNull(result);
        Assert.Equal(new TodoItem("abc", "Buy milk", false), result);

        mockSet.Verify(m => m.Find(It.Is<string>(x => x == "abc")), Times.Once);
    }

    [Fact]
    public void ShouldFetchAllTodos()
    {
        var mockSet = new Mock<DbSet<TodoItem>>();
        var mockContext = new Mock<AppContext>(new DbContextOptions<AppContext>());

        List<TodoItem> items = [
            new TodoItem("abc", "Buy milk", false),
            new TodoItem("def", "Buy bread", true),
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
          new TodoItem("abc", "Buy milk", false),
          new TodoItem("def", "Buy bread", true),
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
        var item = new TodoItem("abc", "Buy milk", false);

        await underTest.Create(item);

        mockSet.Verify(m => m.Add(It.Is<TodoItem>(x => x == item)), Times.Once);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ShouldEditATodo()
    {
        var item = new TodoItem("abc", "Buy milk", false);

        var mockSet = new Mock<DbSet<TodoItem>>();
        var mockContext = new Mock<AppContext>(new DbContextOptions<AppContext>());

        mockContext.Setup(c => c.TodoItems).Returns(mockSet.Object);
        mockSet.Setup(m => m.Find(It.Is<string>(x => x == "abc"))).Returns(new TodoItem("abc", "Buy milk", false));

        var underTest = new TodoService(mockContext.Object);
        await underTest.Create(item);

        var updatedItem = new TodoItem("abc", "Buy eggs", true);
        await underTest.Edit("abc", updatedItem);

        mockSet.Verify(m => m.Find(It.Is<string>(x => x == "abc")), Times.Once);
        mockSet.Verify(m => m.Update(It.Is<TodoItem>(x => x.Equals(updatedItem))), Times.Once);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Fact]
    public async Task ShouldDeleteATodo()
    {
        var mockSet = new Mock<DbSet<TodoItem>>();
        var mockContext = new Mock<AppContext>(new DbContextOptions<AppContext>());

        mockContext.Setup(c => c.TodoItems).Returns(mockSet.Object);
        mockSet.Setup(m => m.Find(It.Is<string>(x => x == "abc"))).Returns(new TodoItem("abc", "Buy milk", false));

        var underTest = new TodoService(mockContext.Object);
        await underTest.Delete("abc");

        mockSet.Verify(m => m.Find(It.Is<string>(x => x == "abc")), Times.Once);
        mockSet.Verify(m => m.Remove(It.Is<TodoItem>(x => x.Key == "abc")), Times.Once);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
