using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using LasPollasHermanas.Client;
using LasPollasHermanas.Client.Data;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using LasPollasHermanas.Client.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var httpClientDildo = new HttpClient { BaseAddress = new Uri("http://localhost:5222") };
var httpClientUser = new HttpClient { BaseAddress = new Uri("http://localhost:5007") };

builder.Services.AddScoped(sp => httpClientUser);
builder.Services.AddScoped(sp => httpClientDildo);

builder.Services.AddScoped<UserClient>(sp => new UserClient(httpClientUser));
builder.Services.AddScoped<DildoClient>(sp => new DildoClient(httpClientDildo));

builder.Services.AddBlazoredSessionStorage();
builder.Services.AddScoped<AuthenticationStateProvider, AuthExtension>();
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
