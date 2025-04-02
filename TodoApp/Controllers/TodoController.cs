using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.Controllers;

[ApiController]
[Route("/api/todo")]
public class TodoController(ITodoService service) : ControllerBase
{
    [HttpGet]
    public ActionResult<List<TodoItem>> GetAll(string q = "")
    {
        return Ok(service.GetAll(q));
    }

    [HttpGet("{id}")]
    public ActionResult<TodoItem> GetOne(long id)
    {
        var todo = service.Get(id);
        return todo == null ? NotFound() : Ok(todo);
    }

    [HttpPost]
    public async Task<ActionResult<TodoItem>> Create([FromBody] TodoItem item)
    {
        var result = await service.Create(item);
        // Using no param Created() constructor causes 204 to be returned due to a alleged bug. @see https://github.com/Azure/Azure-Functions/issues/2475
        return result.Success ? Created("", null) : StatusCode(StatusCodes.Status500InternalServerError, result.Message);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Edit(long id, [FromBody] TodoItem item)
    {
        var result = await service.Edit(id, item);
        return result.Success ? Accepted() : StatusCode(StatusCodes.Status500InternalServerError, result.Message);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<List<TodoItem>>> Delete(long id)
    {
        var result = await service.Delete(id);
        return result.Success ? Accepted() : StatusCode(StatusCodes.Status500InternalServerError, result.Message);
    }
}
