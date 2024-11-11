using Blazored.Modal;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SnusPunch.Web;
using SnusPunch.Web.Clients;
using SnusPunch.Web.Clients.Snus;
using SnusPunch.Web.ViewModels.Snus;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

#region Blazored
builder.Services.AddBlazoredModal();
builder.Services.AddBlazoredToast();
#endregion

#region Clients
string sBaseUrl = builder.Configuration.GetValue<string>("BaseUrl");

builder.Services.AddHttpClient(HttpClientEnum.Auth.ToString(), config => config.BaseAddress = new Uri(sBaseUrl + "Auth/"));
builder.Services.AddScoped<AuthClient>();

builder.Services.AddHttpClient(HttpClientEnum.Snus.ToString(), config => config.BaseAddress = new Uri(sBaseUrl + "Snus/"));
builder.Services.AddScoped<SnusClient>();
#endregion

#region ViewModels
builder.Services.AddScoped<AuthViewModel>();
builder.Services.AddScoped<SnusViewModel>();
#endregion

await builder.Build().RunAsync();
