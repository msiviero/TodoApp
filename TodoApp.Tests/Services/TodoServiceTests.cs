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
        var mockTimeService = new Mock<ITimeService>();

        mockSet.Setup(m => m.Find(It.Is<string>(x => x == "abc"))).Returns(Todo.Create("abc", new DateTime(2023, 1, 1), new DateTime(2023, 1, 1), "Buy milk", false));
        mockContext.Setup(c => c.TodoItems).Returns(mockSet.Object);
        mockTimeService.Setup(m => m.Now()).Returns(new DateTime(2023, 1, 1));


        var underTest = new TodoService(mockContext.Object, mockTimeService.Object);
        var result = underTest.Get("abc");

        Assert.NotNull(result);
        Assert.Equal(Todo.Create("abc", new DateTime(2023, 1, 1), new DateTime(2023, 1, 1), "Buy milk", false), result);

        mockSet.Verify(m => m.Find(It.Is<string>(x => x == "abc")), Times.Once);
    }

    [Fact]
    public void ShouldFetchAllTodos()
    {
        var mockSet = new Mock<DbSet<Todo>>();
        var mockContext = new Mock<TodoAppContext>(new DbContextOptions<TodoAppContext>());
        var mockTimeService = new Mock<ITimeService>();

        List<Todo> items = [
             Todo.Create("abc", new DateTime(2023, 1, 1), new DateTime(2023, 1, 1), "Buy milk", false),
             Todo.Create("def", new DateTime(2023, 1, 1), new DateTime(2023, 1, 1), "Buy bread", true),
        ];

        mockSet.Setup(m => m.AsQueryable()).Returns(items.AsQueryable());
        mockContext.Setup(c => c.TodoItems).Returns(mockSet.Object);
        mockTimeService.Setup(m => m.Now()).Returns(new DateTime(2023, 1, 1));

        var underTest = new TodoService(mockContext.Object, mockTimeService.Object);
        var result = underTest.GetAll();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void ShouldFilterTodos()
    {
        var mockSet = new Mock<DbSet<Todo>>();
        var mockContext = new Mock<TodoAppContext>(new DbContextOptions<TodoAppContext>());
        var mockTimeService = new Mock<ITimeService>();

        List<Todo> items = [
           Todo.Create("abc", new DateTime(2023, 1, 1), new DateTime(2023, 1, 1), "Buy milk", false),
           Todo.Create("def", new DateTime(2023, 1, 1), new DateTime(2023, 1, 1),"Buy bread", true),
       ];

        mockSet.Setup(m => m.AsQueryable()).Returns(items.AsQueryable());
        mockContext.Setup(c => c.TodoItems).Returns(mockSet.Object);
        mockTimeService.Setup(m => m.Now()).Returns(new DateTime(2023, 1, 1));

        var underTest = new TodoService(mockContext.Object, mockTimeService.Object);
        var result = underTest.GetAll("milk");

        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task ShouldCreateATodo()
    {
        var mockSet = new Mock<DbSet<Todo>>();
        var mockContext = new Mock<TodoAppContext>(new DbContextOptions<TodoAppContext>());
        var mockTimeService = new Mock<ITimeService>();

        mockContext.Setup(c => c.TodoItems).Returns(mockSet.Object);

        var underTest = new TodoService(mockContext.Object, mockTimeService.Object);
        var item = Todo.Create("abc", new DateTime(2023, 1, 1), new DateTime(2023, 1, 1), "Buy milk", false);
        mockTimeService.Setup(m => m.Now()).Returns(new DateTime(2023, 1, 1));

        await underTest.Create(item);

        mockSet.Verify(m => m.Add(It.Is<Todo>(x => x == item)), Times.Once);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ShouldEditATodo()
    {
        var item = Todo.Create("abc", new DateTime(2023, 1, 1), new DateTime(2023, 1, 1), "Buy milk", false);

        var mockSet = new Mock<DbSet<Todo>>();
        var mockContext = new Mock<TodoAppContext>(new DbContextOptions<TodoAppContext>());
        var mockTimeService = new Mock<ITimeService>();

        mockContext.Setup(c => c.TodoItems).Returns(mockSet.Object);
        mockSet.Setup(m => m.Find(It.Is<string>(x => x == "abc"))).Returns(Todo.Create("abc", new DateTime(2023, 1, 1), new DateTime(2023, 1, 1), "Buy milk", false));
        mockTimeService.Setup(m => m.Now()).Returns(new DateTime(2023, 1, 1));

        var underTest = new TodoService(mockContext.Object, mockTimeService.Object);
        await underTest.Create(item);

        var updatedItem = Todo.Create("abc", new DateTime(2023, 1, 1), new DateTime(2023, 1, 1), "Buy eggs", true);
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
        var mockTimeService = new Mock<ITimeService>();

        mockContext.Setup(c => c.TodoItems).Returns(mockSet.Object);
        mockSet.Setup(m => m.Find(It.Is<string>(x => x == "abc"))).Returns(Todo.Create("abc", new DateTime(2023, 1, 1), new DateTime(2023, 1, 1), "Buy milk", false));
        mockTimeService.Setup(m => m.Now()).Returns(new DateTime(2023, 1, 1));

        var underTest = new TodoService(mockContext.Object, mockTimeService.Object);
        await underTest.Delete("abc");

        mockSet.Verify(m => m.Find(It.Is<string>(x => x == "abc")), Times.Once);
        mockSet.Verify(m => m.Remove(It.Is<Todo>(x => x.Key == "abc")), Times.Once);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
