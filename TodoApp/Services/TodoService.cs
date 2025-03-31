
using TodoApp.Models;

namespace TodoApp.Services;


public interface ITodoService
{
    List<TodoItem> GetAll();
    TodoItem? Get(long id);
    Task<TodoItem> Create(TodoItem item);
    TodoItem Edit(long id, TodoItem item);
    void Delete(long id);
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

    public void Delete(long id)
    {
        throw new NotImplementedException();
    }

    public TodoItem Edit(long id, TodoItem item)
    {
        throw new NotImplementedException();
    }
}
