namespace TodoApp.Tests.Services;

using TodoApp.Models;
using TodoApp.Services;
using Moq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

// https://learn.microsoft.com/en-us/ef/ef6/fundamentals/testing/mocking?redirectedfrom=MSDN

public class TodoServiceTests
{
    [Fact]
    public void ShouldFetchOneUser()
    {
        var mockSet = new Mock<DbSet<TodoItem>>();
        var mockContext = new Mock<AppContext>(new DbContextOptions<AppContext>());

        mockSet.Setup(m => m.Find(It.Is<long>(x => x == 7))).Returns(new TodoItem(Id: 7, Title: "Buy milk", IsCompleted: false));
        mockContext.Setup(c => c.TodoItems).Returns(mockSet.Object);

        var underTest = new TodoService(mockContext.Object);

        var result = underTest.Get(7);

        Assert.NotNull(result);
        Assert.Equal(new TodoItem(Id: 7, Title: "Buy milk", IsCompleted: false), result);
    }

    [Fact]
    public void ShouldFetchAllUsers()
    {
        var mockSet = new Mock<DbSet<TodoItem>>();
        var mockContext = new Mock<AppContext>(new DbContextOptions<AppContext>());

        List<TodoItem> items = [
            new(Id: 7,  Title: "Buy milk",  IsCompleted: false),
            new(Id: 17, Title: "Buy bread", IsCompleted: true)
        ];

        mockSet.Setup(m => m.AsQueryable()).Returns(items.AsQueryable());
        mockContext.Setup(c => c.TodoItems).Returns(mockSet.Object);

        var underTest = new TodoService(mockContext.Object);
        var result = underTest.GetAll();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task ShouldCreateAUser()
    {
        var mockSet = new Mock<DbSet<TodoItem>>();
        var mockContext = new Mock<AppContext>(new DbContextOptions<AppContext>());

        mockContext.Setup(c => c.TodoItems).Returns(mockSet.Object);

        var underTest = new TodoService(mockContext.Object);
        var item = new TodoItem(Id: 7, Title: "Buy milk", IsCompleted: false);

        await underTest.Create(item);

        mockSet.Verify(m => m.Add(It.Is<TodoItem>(x => x == item)), Times.Once);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
