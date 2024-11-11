using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SnusPunch.Data.DbContexts;
using SnusPunch.Data.Repository;
using SnusPunch.Services.Snus;
using SnusPunch.Shared.Models.Identity;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Auth
builder.Services.AddAuthorization();
builder.Services.AddIdentityCore<SnusPunchUserModel>().AddEntityFrameworkStores<SnusPunchDbContext>();
#endregion

#region Database
builder.Services.AddDbContext<SnusPunchDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("SnusPunch")));

builder.Services.AddScoped<SnusPunchRepository>();
#endregion

#region Services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<SnusService>();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
