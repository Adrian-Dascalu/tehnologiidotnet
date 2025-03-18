using tehnologiinet;
using tehnologiinet.Interfaces;
using tehnologiinet.Repositories;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IMyFirstServiceInterface, MyFirstService>();
builder.Services.AddScoped<IStudentsRepository, StudentsRepository>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql("Host=localhost, Database=postgres, Username=postgres, Password=postgres");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();