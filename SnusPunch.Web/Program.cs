using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SnusPunch.Web;
using SnusPunch.Web.Clients;
using SnusPunch.Web.ViewModels.Snus;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

#region Clients
builder.Services.AddScoped<SnusClient>();
#endregion

#region ViewModels
builder.Services.AddScoped<SnusViewModel>();
#endregion

await builder.Build().RunAsync();
