using Blazored.Modal;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SnusPunch.Web;
using SnusPunch.Web.Clients;
using SnusPunch.Web.Clients.Snus;
using SnusPunch.Web.Identity;
using SnusPunch.Web.ViewModels.Snus;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

#region Auth
builder.Services.AddTransient<CookieHandler>();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<CookieAuthenticationStateProvider>();

builder.Services.AddScoped<AuthenticationStateProvider>(
  p => p.GetService<CookieAuthenticationStateProvider>());
#endregion

#region Blazored
builder.Services.AddBlazoredModal();
builder.Services.AddBlazoredToast();
#endregion

#region Clients
string sBaseUrl = builder.Configuration.GetValue<string>("BaseUrl");
builder.Services.AddHttpClient(HttpClientEnum.Base.ToString(), config => config.BaseAddress = new Uri(sBaseUrl)).AddHttpMessageHandler<CookieHandler>();

builder.Services.AddHttpClient(HttpClientEnum.Auth.ToString(), config => config.BaseAddress = new Uri(sBaseUrl + "Auth/")).AddHttpMessageHandler<CookieHandler>();
builder.Services.AddScoped<AuthClient>();

builder.Services.AddHttpClient(HttpClientEnum.Snus.ToString(), config => config.BaseAddress = new Uri(sBaseUrl + "Snus/")).AddHttpMessageHandler<CookieHandler>();
builder.Services.AddScoped<SnusClient>();

builder.Services.AddHttpClient(HttpClientEnum.User.ToString(), config => config.BaseAddress = new Uri(sBaseUrl + "User/")).AddHttpMessageHandler<CookieHandler>();
builder.Services.AddScoped<UserClient>();
#endregion

#region ViewModels
builder.Services.AddScoped<AuthViewModel>();
builder.Services.AddScoped<SnusViewModel>();
builder.Services.AddScoped<UserViewModel>();
#endregion

await builder.Build().RunAsync();
