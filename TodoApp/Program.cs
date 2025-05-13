using TodoApp.Services;
using Microsoft.EntityFrameworkCore;

var devCorsPolicyName = "_dev";
var prodCorsPolicyName = "_prod";

var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy(devCorsPolicyName, policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
    options.AddPolicy(prodCorsPolicyName, policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddDbContext<TodoApp.Models.TodoAppContext>(opt =>
{
    var connection = Environment.GetEnvironmentVariable("PG_CONNECTION") ?? "Server=127.0.0.1;Port=5432;Database=todos;User Id=postgres;Password=password;";
    opt.UseNpgsql(connection);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ITodoService, TodoService>();
builder.Services.AddSingleton<ITimeService, TimeService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(devCorsPolicyName);
}
else
{
    app.UseCors(prodCorsPolicyName);
}

app.MapControllers();
app.Run($"http://0.0.0.0:{port}");
