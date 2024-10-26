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

#region Clients
string sBaseUrl = builder.Configuration.GetValue<string>("BaseUrl");

builder.Services.AddHttpClient(HttpClientEnum.Snus.ToString(), config => config.BaseAddress = new Uri(sBaseUrl + "Snus/"));
builder.Services.AddScoped<SnusClient>();
#endregion

#region ViewModels
builder.Services.AddScoped<SnusViewModel>();
#endregion

await builder.Build().RunAsync();
