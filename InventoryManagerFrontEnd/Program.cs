using InventoryManagerFrontEnd;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using InventoryManagerFrontEnd.Interfaces;
using InventoryManagerFrontEnd.Services;
using InventoryManagerFrontEnd.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Security;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAPIHostUrl, APIHostUrl>();


builder.Services.AddAuthorizationCore();


var host = builder.Build();

var tokenService = host.Services.GetRequiredService<ITokenService>();
await tokenService.OnInitialize();

await host.RunAsync();

