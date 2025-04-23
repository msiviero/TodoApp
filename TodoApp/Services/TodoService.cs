
using TodoApp.Models;

namespace TodoApp.Services;


public interface ITodoService
{
    List<TodoItem> GetAll(string query = "");
    TodoItem? Get(string key);
    Task<UpdateStatus> Create(TodoItem item);
    Task<UpdateStatus> Edit(string id, TodoItem item);
    Task<UpdateStatus> Delete(string id);
}

public class TodoService(Models.AppContext _ctx) : ITodoService
{
    public List<TodoItem> GetAll(string query = "")
    {
        var items = _ctx.TodoItems
            .AsQueryable()
            .Where(x => x.Title.Contains(query));

        return [.. items];
    }

    public TodoItem? Get(string key)
    {
        return _ctx.TodoItems.Find(key);
    }

    public async Task<UpdateStatus> Create(TodoItem item)
    {
        _ctx.TodoItems.Add(item);
        await _ctx.SaveChangesAsync();
        return new UpdateStatus(true, "Todo created");
    }

    public async Task<UpdateStatus> Delete(string key)
    {
        var item = _ctx.TodoItems.Find(key);
        if (item == null)
        {
            return new UpdateStatus(false, $"Todo with key:{key} not found");
        }
        _ctx.TodoItems.Remove(item);
        await _ctx.SaveChangesAsync();
        return new UpdateStatus(true, "Todo deleted");
    }

    public async Task<UpdateStatus> Edit(string key, TodoItem item)
    {
        if (key != item.Key)
        {
            return new UpdateStatus(false, "Key does not match");
        }

        var it = _ctx.TodoItems.Find(key);
        if (it == null)
        {
            return new UpdateStatus(false, $"Todo with key:{key} not found");
        }
        _ctx.TodoItems.Update(new TodoItem(key, item.Title, item.IsCompleted));
        await _ctx.SaveChangesAsync();
        return new UpdateStatus(true, "Todo updated");
    }
}

public record class UpdateStatus(bool Success, string Message);
