using Microsoft.EntityFrameworkCore;
using SnusPunch.Data.DbContexts;
using SnusPunch.Data.Repository;
using SnusPunch.Services.Snus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Database
builder.Services.AddDbContext<SnusPunchDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("SnusPunch")));

builder.Services.AddScoped<SnusPunchRepository>();
#endregion

#region Services
builder.Services.AddScoped<SnusService>();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(options => options.AllowAnyOrigin());
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
