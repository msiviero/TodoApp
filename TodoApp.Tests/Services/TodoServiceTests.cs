namespace TodoApp.Tests.Services;

using TodoApp.Models;
using TodoApp.Services;
using Moq;
using Microsoft.EntityFrameworkCore;

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
}

// https://learn.microsoft.com/en-us/ef/ef6/fundamentals/testing/mocking?redirectedfrom=MSDN