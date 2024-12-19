using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using SnusPunch.Data.DbContexts;
using SnusPunch.Data.Models.Identity;
using SnusPunch.Data.Repository;
using SnusPunch.Services.Email;
using SnusPunch.Services.Entry;
using SnusPunch.Services.NotificationService;
using SnusPunch.Services.Snus;
using SnusPunch.Services.Statistics;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;

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

    options.OrderActionsBy((sApiDesc) =>
    {
        var sMethodOrder = new Dictionary<string, int> 
        {
            { "GET", 1 },
            { "POST", 2 },
            { "PUT", 3 },
            { "DELETE", 4 }
        };

        var sMethodRank = sMethodOrder.ContainsKey(sApiDesc.HttpMethod) ? sMethodOrder[sApiDesc.HttpMethod] : int.MaxValue;

        return $"{sApiDesc.ActionDescriptor.RouteValues["controller"]}_{sMethodRank}_{sApiDesc.ActionDescriptor.RouteValues["action"] ?? string.Empty}";
    });
});

builder.Services.AddSignalR();

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

builder.Services.AddScoped<EntryService>();
builder.Services.AddScoped<EntryLikeService>();

builder.Services.AddScoped<EntryCommentService>();
builder.Services.AddScoped<EntryCommentLikeService>();

builder.Services.AddScoped<FriendService>();

builder.Services.AddScoped<NotificationService>();

builder.Services.AddScoped<SnusService>();
builder.Services.AddScoped<StatisticsService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<NotificationHub>();
#endregion

//Load Smtp Credentials
builder.Configuration.AddJsonFile("smtpsettings.json", false);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x => x.DocExpansion(DocExpansion.None));
    app.UseCors(options => options.WithOrigins(builder.Configuration["BackendUrl"], builder.Configuration["FrontendUrl"]).AllowAnyHeader().AllowAnyMethod().AllowCredentials());
}

app.UseHttpsRedirection();

#region Images
app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory
    (), "images")),
    RequestPath = "/images"
});
#endregion

app.UseAuthorization();

app.MapControllers();

app.MapHub<NotificationHub>("Notifications");

app.Run();
