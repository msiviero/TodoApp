using Microsoft.AspNetCore.Mvc;
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
    public ActionResult<List<TodoItem>> Get(long id)
    {
        return Ok(service.Get(id));
    }

    [HttpPost]
    public ActionResult<TodoItem> Create([FromBody] TodoItem item)
    {
        service.Create(item);
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
