
namespace TodoApp.Services;


public interface ITodoService
{
    List<TodoItem> GetAll();
    TodoItem Get(long id);
    TodoItem Create(TodoItem item);
    TodoItem Edit(long id, TodoItem item);
    void Delete(long id);
}

public class TodoService : ITodoService
{
    public TodoItem Create(TodoItem item)
    {
        throw new NotImplementedException();
    }

    public void Delete(long id)
    {
        throw new NotImplementedException();
    }

    public TodoItem Edit(long id, TodoItem item)
    {
        throw new NotImplementedException();
    }

    public TodoItem Get(long id)
    {
        throw new NotImplementedException();
    }

    public List<TodoItem> GetAll()
    {
        throw new NotImplementedException();
    }
}

public record TodoItem(string Title, bool IsCompleted);
