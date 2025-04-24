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

    [HttpGet("{key}")]
    public ActionResult<TodoItem> GetOne(string key)
    {
        var todo = service.Get(key);
        return todo == null ? NotFound() : Ok(todo);
    }

    [HttpPost]
    public async Task<ActionResult<TodoItem>> Create([FromBody] TodoItem item)
    {
        try
        {
            var result = await service.Create(item);
            // Using no param Created() constructor causes 204 to be returned due to a alleged bug. @see https://github.com/Azure/Azure-Functions/issues/2475
            return result.Success ? Created("", null) : StatusCode(StatusCodes.Status500InternalServerError, result.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPut("{key}")]
    public async Task<ActionResult> Edit(string key, [FromBody] TodoItem item)
    {
        try
        {
            var result = await service.Edit(key, item);
            return result.Success ? Accepted() : StatusCode(StatusCodes.Status500InternalServerError, result.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpDelete("{key}")]
    public async Task<ActionResult<List<TodoItem>>> Delete(string key)
    {
        try
        {
            var result = await service.Delete(key);
            return result.Success ? Accepted() : StatusCode(StatusCodes.Status500InternalServerError, result.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
