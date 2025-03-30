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
    public ActionResult<List<TodoItem>> GetAll()
    {
        return Ok(service.GetAll());
    }

    [HttpGet("{id}")]
    public ActionResult<TodoItem> Get(long id)
    {
        var todo = service.Get(id);
        if (todo == null) return NotFound();
        return todo;
    }

    [HttpPost]
    public async Task<ActionResult<TodoItem>> Create([FromBody] TodoItem item)
    {
        await service.Create(item);
        return Created();
    }

    [HttpPut("{id}")]
    public ActionResult<List<TodoItem>> Edit(long id, [FromBody] TodoItem item)
    {
        return Ok(service.Edit(id, item));
    }

    [HttpDelete("{id}")]
    public ActionResult<List<TodoItem>> Delete(long id)
    {
        service.Delete(id);
        return Ok();
    }
}
