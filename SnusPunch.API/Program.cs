using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SnusPunch.Data.DbContexts;
using SnusPunch.Data.Models.Identity;
using SnusPunch.Data.Repository;
using SnusPunch.Services.Email;
using SnusPunch.Services.Snus;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

#region Auth
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<SnusPunchUserModel>
    (opt =>
    {
        opt.User.RequireUniqueEmail = true;
        opt.Lockout = new LockoutOptions
        {
            DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5),
            AllowedForNewUsers = true,
            MaxFailedAccessAttempts = 5
        };
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<SnusPunchDbContext>();

builder.Services.Configure<DataProtectionTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromHours(1));
#endregion

#region Database
builder.Services.AddDbContext<SnusPunchDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("SnusPunch")));

builder.Services.AddScoped<SnusPunchRepository>();
#endregion

#region Services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<SnusService>();
builder.Services.AddScoped<UserService>();
#endregion

//Load Smtp Credentials
builder.Configuration.AddJsonFile("smtpsettings.json", false);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(options => options.WithOrigins(builder.Configuration["BackendUrl"], builder.Configuration["FrontendUrl"]).AllowAnyHeader().AllowAnyMethod().AllowCredentials());
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
