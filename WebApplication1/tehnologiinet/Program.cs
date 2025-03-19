using Microsoft.EntityFrameworkCore;
using tehnologiinet;
using tehnologiinet.Interfaces;
using tehnologiinet.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IMyFirstServiceInterface, MyFirstService>();
builder.Services.AddScoped<IStudentsRepository, StudentsRepository>();
builder.Services.AddDbContext<DatabaseContext>(options => 
    options.UseNpgsql("Host=localhost;Database=tehnologiinet;Username=postgres;Password=parkingshare"));

var app = builder.Build();

using (var db = new DatabaseContext())
{
   db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();