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
            TodoItem.Create("abc", "Buy milk", false),
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
        mockService.Setup(s => s.Get(It.Is<string>(x => x == "abc"))).Returns(TodoItem.Create("abc", "Buy milk", false));

        var underTest = new TodoController(mockService.Object);
        var result = (OkObjectResult?)underTest.GetOne("abc").Result;

        Assert.Equal(TodoItem.Create("abc", "Buy milk", false), result?.Value);
        Assert.Equal(StatusCodes.Status200OK, result?.StatusCode);

        mockService.Verify(s => s.Get(It.Is<string>(x => x == "abc")), Times.Once);
    }

    [Fact]
    public void ShouldReturnNotFoundWhenTodoDoesNotExist()
    {
        var mockService = new Mock<ITodoService>();

        var underTest = new TodoController(mockService.Object);
        var result = (NotFoundResult?)underTest.GetOne("abc").Result;

        Assert.Equal(StatusCodes.Status404NotFound, result?.StatusCode);
        mockService.Verify(s => s.Get(It.Is<string>(x => x == "abc")), Times.Once);
    }

    [Fact]
    public async Task ShouldReturnCreatedWhenTodoIsCreated()
    {
        var mockService = new Mock<ITodoService>();
        var item = TodoItem.Create("abc", "Buy milk", false);
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
        var item = TodoItem.Create("abc", "Buy milk", false);
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
        var item = TodoItem.Create("abc", "Buy milk", false);

        mockService
            .Setup(s => s.Edit(
                It.Is<string>(x => x == "abc"),
                It.Is<TodoItem>(x => x == item))
            )
            .ReturnsAsync(new UpdateStatus(Success: true, Message: "Success"));

        var underTest = new TodoController(mockService.Object);
        var result = await underTest.Edit("abc", item);
        var actualResult = (AcceptedResult?)result;

        Assert.Equal(StatusCodes.Status202Accepted, actualResult?.StatusCode);
    }

    [Fact]
    public async Task ShouldReturnAnErrorWhenTodoCantBeEdited()
    {
        var mockService = new Mock<ITodoService>();
        var item = TodoItem.Create("abc", "Buy milk", false);

        mockService
            .Setup(s => s.Edit(
                It.Is<string>(x => x == "abc"),
                It.Is<TodoItem>(x => x == item))
            )
            .ReturnsAsync(new UpdateStatus(Success: false, Message: "Error"));

        var underTest = new TodoController(mockService.Object);
        var result = await underTest.Edit("abc", item);
        var actualResult = (ObjectResult?)result;

        Assert.Equal(StatusCodes.Status500InternalServerError, actualResult?.StatusCode);
        Assert.Equal("Error", actualResult?.Value);
    }

    [Fact]
    public async Task ShouldReturnOkWhenATodoIsDeleted()
    {
        var mockService = new Mock<ITodoService>();
        var item = TodoItem.Create("abc", "Buy milk", false);

        mockService
            .Setup(s => s.Delete(It.Is<string>(x => x == "abc")))
            .ReturnsAsync(new UpdateStatus(Success: true, Message: "Success"));

        var underTest = new TodoController(mockService.Object);
        var result = await underTest.Delete("abc");
        var actualResult = (AcceptedResult?)result.Result;

        Assert.Equal(StatusCodes.Status202Accepted, actualResult?.StatusCode);
    }

    [Fact]
    public async Task ShouldReturnAnErrorWhenATodoCantBeDeleted()
    {
        var mockService = new Mock<ITodoService>();
        var item = TodoItem.Create("abc", "Buy milk", false);

        mockService
            .Setup(s => s.Delete(It.Is<string>(x => x == "abc")))
            .ReturnsAsync(new UpdateStatus(Success: false, Message: "Error"));

        var underTest = new TodoController(mockService.Object);
        var result = await underTest.Delete("abc");
        var actualResult = (ObjectResult?)result.Result;

        Assert.Equal(StatusCodes.Status500InternalServerError, actualResult?.StatusCode);
        Assert.Equal("Error", actualResult?.Value);
    }
}
