using Blazored.Modal;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using SnusPunch.Web;
using SnusPunch.Web.Clients;
using SnusPunch.Web.Clients.Notification;
using SnusPunch.Web.Clients.Snus;
using SnusPunch.Web.Identity;
using SnusPunch.Web.ViewModels;
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

builder.Services.AddHttpClient(HttpClientEnum.Auth.ToString(), config => config.BaseAddress = new Uri(sBaseUrl + "Auth/")).AddHttpMessageHandler<CookieHandler>();
builder.Services.AddScoped<AuthClient>();

builder.Services.AddHttpClient(HttpClientEnum.Entry.ToString(), config => config.BaseAddress = new Uri(sBaseUrl + "Entry/")).AddHttpMessageHandler<CookieHandler>();
builder.Services.AddHttpClient(HttpClientEnum.EntryLike.ToString(), config => config.BaseAddress = new Uri(sBaseUrl + "EntryLike/")).AddHttpMessageHandler<CookieHandler>();
builder.Services.AddHttpClient(HttpClientEnum.EntryComment.ToString(), config => config.BaseAddress = new Uri(sBaseUrl + "EntryComment/")).AddHttpMessageHandler<CookieHandler>();
builder.Services.AddHttpClient(HttpClientEnum.EntryCommentLike.ToString(), config => config.BaseAddress = new Uri(sBaseUrl + "EntryCommentLike/")).AddHttpMessageHandler<CookieHandler>();
builder.Services.AddScoped<EntryClient>();

builder.Services.AddHttpClient(HttpClientEnum.Friend.ToString(), config => config.BaseAddress = new Uri(sBaseUrl + "Friend/")).AddHttpMessageHandler<CookieHandler>();
builder.Services.AddScoped<FriendClient>();

builder.Services.AddHttpClient(HttpClientEnum.Notification.ToString(), config => config.BaseAddress = new Uri(sBaseUrl + "Notification/")).AddHttpMessageHandler<CookieHandler>();
builder.Services.AddScoped<NotificationClient>();

builder.Services.AddHttpClient(HttpClientEnum.Snus.ToString(), config => config.BaseAddress = new Uri(sBaseUrl + "Snus/")).AddHttpMessageHandler<CookieHandler>();
builder.Services.AddScoped<SnusClient>();

builder.Services.AddHttpClient(HttpClientEnum.Statistics.ToString(), config => config.BaseAddress = new Uri(sBaseUrl + "Statistics/")).AddHttpMessageHandler<CookieHandler>();
builder.Services.AddScoped<StatisticsClient>();

builder.Services.AddHttpClient(HttpClientEnum.User.ToString(), config => config.BaseAddress = new Uri(sBaseUrl + "User/")).AddHttpMessageHandler<CookieHandler>();
builder.Services.AddScoped<UserClient>();
#endregion


#region ViewModels
builder.Services.AddScoped<AuthViewModel>();
builder.Services.AddScoped<BaseViewModel>();
builder.Services.AddScoped<EntryViewModel>();
builder.Services.AddScoped<SnusViewModel>();
builder.Services.AddScoped<StatisticsViewModel>();
builder.Services.AddScoped<UserViewModel>();
#endregion


await builder.Build().RunAsync();
