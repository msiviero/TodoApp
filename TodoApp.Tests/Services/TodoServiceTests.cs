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
        var mockSet = new Mock<DbSet<Todo>>();
        var mockContext = new Mock<TodoAppContext>(new DbContextOptions<TodoAppContext>());

        mockSet.Setup(m => m.Find(It.Is<string>(x => x == "abc"))).Returns( Todo.Create("abc", "Buy milk", false));
        mockContext.Setup(c => c.TodoItems).Returns(mockSet.Object);

        var underTest = new TodoService(mockContext.Object);

        var result = underTest.Get("abc");

        Assert.NotNull(result);
        Assert.Equal(Todo.Create("abc", "Buy milk", false), result);

        mockSet.Verify(m => m.Find(It.Is<string>(x => x == "abc")), Times.Once);
    }

    [Fact]
    public void ShouldFetchAllTodos()
    {
        var mockSet = new Mock<DbSet<Todo>>();
        var mockContext = new Mock<TodoAppContext>(new DbContextOptions<TodoAppContext>());

        List<Todo> items = [
             Todo.Create("abc", "Buy milk", false),
             Todo.Create("def", "Buy bread", true),
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
        var mockSet = new Mock<DbSet<Todo>>();
        var mockContext = new Mock<TodoAppContext>(new DbContextOptions<TodoAppContext>());

        List<Todo> items = [
           Todo.Create("abc", "Buy milk", false),
           Todo.Create("def", "Buy bread", true),
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
        var mockSet = new Mock<DbSet<Todo>>();
        var mockContext = new Mock<TodoAppContext>(new DbContextOptions<TodoAppContext>());

        mockContext.Setup(c => c.TodoItems).Returns(mockSet.Object);

        var underTest = new TodoService(mockContext.Object);
        var item =  Todo.Create("abc", "Buy milk", false);

        await underTest.Create(item);

        mockSet.Verify(m => m.Add(It.Is<Todo>(x => x == item)), Times.Once);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ShouldEditATodo()
    {
        var item =  Todo.Create("abc", "Buy milk", false);

        var mockSet = new Mock<DbSet<Todo>>();
        var mockContext = new Mock<TodoAppContext>(new DbContextOptions<TodoAppContext>());

        mockContext.Setup(c => c.TodoItems).Returns(mockSet.Object);
        mockSet.Setup(m => m.Find(It.Is<string>(x => x == "abc"))).Returns( Todo.Create("abc", "Buy milk", false));

        var underTest = new TodoService(mockContext.Object);
        await underTest.Create(item);

        var updatedItem =  Todo.Create("abc", "Buy eggs", true);
        await underTest.Edit("abc", updatedItem);

        mockSet.Verify(m => m.Find(It.Is<string>(x => x == "abc")), Times.Once);
        mockSet.Verify(m => m.Update(It.Is<Todo>(x => x.Equals(updatedItem))), Times.Once);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Fact]
    public async Task ShouldDeleteATodo()
    {
        var mockSet = new Mock<DbSet<Todo>>();
        var mockContext = new Mock<TodoAppContext>(new DbContextOptions<TodoAppContext>());

        mockContext.Setup(c => c.TodoItems).Returns(mockSet.Object);
        mockSet.Setup(m => m.Find(It.Is<string>(x => x == "abc"))).Returns( Todo.Create("abc", "Buy milk", false));

        var underTest = new TodoService(mockContext.Object);
        await underTest.Delete("abc");

        mockSet.Verify(m => m.Find(It.Is<string>(x => x == "abc")), Times.Once);
        mockSet.Verify(m => m.Remove(It.Is<Todo>(x => x.Key == "abc")), Times.Once);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
