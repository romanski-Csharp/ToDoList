using ToDoList.Models;
using Microsoft.EntityFrameworkCore;
using ToDoList.Services;

var builder = WebApplication.CreateBuilder(args);

// Додаємо сервіс бази даних
builder.Services.AddSingleton<ToDoService>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseDefaultFiles(); 
app.UseStaticFiles();
app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseAuthorization();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
