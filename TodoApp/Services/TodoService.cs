
using TodoApp.Models;

namespace TodoApp.Services;


public interface ITodoService
{
    List<TodoItem> GetAll();
    TodoItem? Get(long id);
    Task<UpdateStatus> Create(TodoItem item);
    Task<UpdateStatus> Edit(long id, TodoItem item);
    Task<UpdateStatus> Delete(long id);
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

    public async Task<UpdateStatus> Create(TodoItem item)
    {
        _ctx.TodoItems.Add(item);
        await _ctx.SaveChangesAsync();
        return new UpdateStatus(true, "Todo created");
    }

    public async Task<UpdateStatus> Delete(long id)
    {
        var item = _ctx.TodoItems.Find(id);
        if (item == null)
        {
            return new UpdateStatus(false, $"Todo with id:{id} not found");
        }
        _ctx.TodoItems.Remove(item);
        await _ctx.SaveChangesAsync();
        return new UpdateStatus(true, "Todo deleted");
    }

    public async Task<UpdateStatus> Edit(long id, TodoItem item)
    {
        if (id != item.Id)
        {
            return new UpdateStatus(false, "Id does not match");
        }

        var it = _ctx.TodoItems.Find(id);
        if (it == null)
        {
            return new UpdateStatus(false, $"Todo with id:{id} not found");
        }
        _ctx.TodoItems.Update(new TodoItem(Id: id, Title: item.Title, IsCompleted: item.IsCompleted));
        await _ctx.SaveChangesAsync();
        return new UpdateStatus(true, "Todo updated");
    }
}

public record class UpdateStatus(bool Success, string Message);
