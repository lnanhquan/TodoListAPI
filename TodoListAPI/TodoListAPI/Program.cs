using Microsoft.EntityFrameworkCore;
using TodoListAPI.Data;
using TodoListAPI.Repository;
using TodoListAPI.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add Cors
builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));
// Dependency Injection for Repository
builder.Services.AddScoped<IToDoItemRepository, ToDoItemRepository>();
builder.Services.AddScoped<IToDoItemService, ToDoItemService>();

// Connect to SQL Server database
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("MyPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
