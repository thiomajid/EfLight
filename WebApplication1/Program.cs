using EfLight.Extensions;

using Microsoft.EntityFrameworkCore;

using WebApplication1;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddEfLight<Program>(opt =>
{
    opt.DefaultLifetime = ServiceLifetime.Singleton;
});

builder.Services.AddDbContext<MyContext>(opt =>
{
    opt.UseInMemoryDatabase(databaseName: "test");
});

var app = builder.Build();

builder.Services.ToList().ForEach(x => Console.WriteLine(x.ToString()));



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

