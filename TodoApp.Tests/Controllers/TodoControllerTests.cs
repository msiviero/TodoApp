using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Controllers;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.Tests.Controllers;

public class TodoControllerTests
{
    [Fact]
    public void ShouldReturnAListOfTodos()
    {
        var items = new List<TodoItem>
        {
            new(1, "Buy milk", false),
        };

        var mockService = new Mock<ITodoService>();
        mockService.Setup(s => s.GetAll(It.Is<string>(x => x == "milk"))).Returns(items);

        var underTest = new TodoController(mockService.Object);

        var result = underTest.GetAll("milk").Result;
        Assert.IsType<OkObjectResult>(result);

        mockService.Verify(s => s.GetAll(It.Is<string>(x => x == "milk")), Times.Once);
    }

    [Fact]
    public void ShouldReturnOkWithTodoIfTodoExists()
    {
        var mockService = new Mock<ITodoService>();
        mockService.Setup(s => s.Get(It.Is<long>(x => x == 7))).Returns(new TodoItem(Id: 7, Title: "Buy milk", IsCompleted: false));

        var underTest = new TodoController(mockService.Object);
        var result = (OkObjectResult?)underTest.GetOne(7).Result;

        Assert.Equal(new TodoItem(Id: 7, Title: "Buy milk", IsCompleted: false), result?.Value);
        Assert.Equal(StatusCodes.Status200OK, result?.StatusCode);

        mockService.Verify(s => s.Get(It.Is<long>(x => x == 7)), Times.Once);
    }

    [Fact]
    public void ShouldReturnNotFoundWhenTodoDoesNotExist()
    {
        var mockService = new Mock<ITodoService>();

        var underTest = new TodoController(mockService.Object);
        var result = (NotFoundResult?)underTest.GetOne(7).Result;

        Assert.Equal(StatusCodes.Status404NotFound, result?.StatusCode);
        mockService.Verify(s => s.Get(It.Is<long>(x => x == 7)), Times.Once);
    }

    [Fact]
    public async Task ShouldReturnCreatedWhenTodoIsCreated()
    {
        var mockService = new Mock<ITodoService>();
        var item = new TodoItem(Id: 7, Title: "Buy milk", IsCompleted: false);
        mockService.Setup(s => s.Create(It.Is<TodoItem>(x => x == item))).ReturnsAsync(new UpdateStatus(Success: true, Message: "Success"));

        var underTest = new TodoController(mockService.Object);
        var result = await underTest.Create(item);
        var actualResult = (CreatedResult?)result.Result;

        Assert.Equal(StatusCodes.Status201Created, actualResult?.StatusCode);
    }

    [Fact]
    public async Task ShouldReturnAnErrorWhenTodoCantBeCreated()
    {
        var mockService = new Mock<ITodoService>();
        var item = new TodoItem(Id: 7, Title: "Buy milk", IsCompleted: false);
        mockService.Setup(s => s.Create(It.Is<TodoItem>(x => x == item))).ReturnsAsync(new UpdateStatus(Success: false, Message: "Error"));

        var underTest = new TodoController(mockService.Object);
        var result = await underTest.Create(item);
        var actualResult = (ObjectResult?)result.Result;

        Assert.Equal(StatusCodes.Status500InternalServerError, actualResult?.StatusCode);
    }

    [Fact]
    public async Task ShouldReturnAcceptedWhenTodoIsEdited()
    {
        var mockService = new Mock<ITodoService>();
        var item = new TodoItem(Id: 7, Title: "Buy milk", IsCompleted: false);

        mockService
            .Setup(s => s.Edit(
                It.Is<long>(x => x == 7),
                It.Is<TodoItem>(x => x == item))
            )
            .ReturnsAsync(new UpdateStatus(Success: true, Message: "Success"));

        var underTest = new TodoController(mockService.Object);
        var result = await underTest.Edit(7, item);
        var actualResult = (AcceptedResult?)result;

        Assert.Equal(StatusCodes.Status202Accepted, actualResult?.StatusCode);
    }

    [Fact]
    public async Task ShouldReturnAnErrorWhenTodoCantBeEdited()
    {
        var mockService = new Mock<ITodoService>();
        var item = new TodoItem(Id: 7, Title: "Buy milk", IsCompleted: false);

        mockService
            .Setup(s => s.Edit(
                It.Is<long>(x => x == 7),
                It.Is<TodoItem>(x => x == item))
            )
            .ReturnsAsync(new UpdateStatus(Success: false, Message: "Error"));

        var underTest = new TodoController(mockService.Object);
        var result = await underTest.Edit(7, item);
        var actualResult = (ObjectResult?)result;

        Assert.Equal(StatusCodes.Status500InternalServerError, actualResult?.StatusCode);
        Assert.Equal("Error", actualResult?.Value);
    }

    [Fact]
    public async Task ShouldReturnOkWhenATodoIsDeleted()
    {
        var mockService = new Mock<ITodoService>();
        var item = new TodoItem(Id: 7, Title: "Buy milk", IsCompleted: false);

        mockService
            .Setup(s => s.Delete(It.Is<long>(x => x == 7)))
            .ReturnsAsync(new UpdateStatus(Success: true, Message: "Success"));

        var underTest = new TodoController(mockService.Object);
        var result = await underTest.Delete(7);
        var actualResult = (AcceptedResult?)result.Result;

        Assert.Equal(StatusCodes.Status202Accepted, actualResult?.StatusCode);
    }

    [Fact]
    public async Task ShouldReturnAnErrorWhenATodoCantBeDeleted()
    {
        var mockService = new Mock<ITodoService>();
        var item = new TodoItem(Id: 7, Title: "Buy milk", IsCompleted: false);

        mockService
            .Setup(s => s.Delete(It.Is<long>(x => x == 7)))
            .ReturnsAsync(new UpdateStatus(Success: false, Message: "Error"));

        var underTest = new TodoController(mockService.Object);
        var result = await underTest.Delete(7);
        var actualResult = (ObjectResult?)result.Result;

        Assert.Equal(StatusCodes.Status500InternalServerError, actualResult?.StatusCode);
        Assert.Equal("Error", actualResult?.Value);
    }
}
