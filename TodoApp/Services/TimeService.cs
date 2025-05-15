namespace TodoApp.Services;


public interface ITimeService
{
    DateTime Now();
}

public class TimeService : ITimeService
{
    public DateTime Now()
    {
        return DateTime.Now.ToUniversalTime();
    }
}

