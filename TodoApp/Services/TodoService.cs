
using TodoApp.Models;

namespace TodoApp.Services;


public interface ITodoService
{
    List<TodoItem> GetAll();
    TodoItem? Get(long id);
    Task<TodoItem> Create(TodoItem item);
    Task Edit(long id, TodoItem item);
    Task Delete(long id);
}

public class TodoService(Models.AppContext _ctx) : ITodoService
{
    public List<TodoItem> GetAll()
    {
        return [.. _ctx.TodoItems.AsQueryable()];
    }

    public TodoItem? Get(long id)
    {
        return _ctx.TodoItems.Find(id);
    }

    public async Task<TodoItem> Create(TodoItem item)
    {
        _ctx.TodoItems.Add(item);
        await _ctx.SaveChangesAsync();
        return item;
    }

    public Task Delete(long id)
    {
        throw new NotImplementedException();
    }

    public Task Edit(long id, TodoItem item)
    {
        throw new NotImplementedException();
    }
}
