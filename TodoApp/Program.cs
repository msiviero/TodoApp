using TodoApp.Services;
using Microsoft.EntityFrameworkCore;

var devCorsPolicyName = "_dev";
var port = 5000;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy(devCorsPolicyName, policy => policy.AllowAnyOrigin().AllowAnyHeader());
});

builder.Services.AddHealthChecks();
builder.Services.AddDbContext<TodoApp.Models.AppContext>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ITodoService, TodoService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(devCorsPolicyName);
}

app.MapControllers();
app.Run($"http://0.0.0.0:{port}");
