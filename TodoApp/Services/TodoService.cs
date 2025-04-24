
using Microsoft.EntityFrameworkCore;
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
        try
        {
            await _ctx.SaveChangesAsync();
            return new UpdateStatus(true, "Todo deleted");

        }
        catch (DbUpdateException e)
        {
            return new UpdateStatus(false, e.Message);
        }
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

        it.Key = key;
        it.Title = item.Title;
        it.IsCompleted = item.IsCompleted;

        _ctx.TodoItems.Update(it);
        await _ctx.SaveChangesAsync();
        return new UpdateStatus(true, "Todo updated");
    }
}

public record class UpdateStatus(bool Success, string Message);
