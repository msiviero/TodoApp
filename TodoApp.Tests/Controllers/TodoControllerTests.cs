
using TodoApp.Controllers;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.Tests.Controllers;

public class TodoControllerTests
{
    [Fact]
    public void ShouldReturnOkWithTodoIfTodoExists()
    {
        var mockService = new Mock<ITodoService>();
        mockService.Setup(s => s.Get(It.Is<long>(x => x == 7))).Returns(new TodoItem(Id: 7, Title: "Buy milk", IsCompleted: false));

        var underTest = new TodoController(mockService.Object);

        var result = underTest.Get(7);
        Assert.Equal(new TodoItem(Id: 7, Title: "Buy milk", IsCompleted: false), result.Value);

        mockService.Verify(s => s.Get(It.Is<long>(x => x == 7)), Times.Once);
    }

    [Fact]
    public void ShouldReturnNotFoundWhenTodoDoesNotExist()
    {
        var mockService = new Mock<ITodoService>();
        var underTest = new TodoController(mockService.Object);

        var result = underTest.Get(7);
        Assert.Null(result.Value);

        mockService.Verify(s => s.Get(It.Is<long>(x => x == 7)), Times.Once);

    }
}

// https://learn.microsoft.com/en-us/ef/ef6/fundamentals/testing/mocking?redirectedfrom=MSDN