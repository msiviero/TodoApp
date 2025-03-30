namespace TodoApp.Tests;

using TodoApp.Services;

public class TodoServiceTests
{
    [Fact]
    public void ShouldFetchOneUser()
    {
        var underTest = new TodoService();

        var result = underTest.Get(7);

        Assert.NotNull(result);
        Assert.Equal(new TodoItem(Id: 1, Title: "Buy milk", IsCompleted: false), result);
    }
}
