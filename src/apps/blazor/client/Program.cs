using FSH.Starter.Blazor.Client;
using FSH.Starter.Blazor.Infrastructure;
using FSH.Starter.Blazor.Modules;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddClientServices(builder.Configuration);
builder.Services.ConfigureBlazorModules();
var app = builder.Build();
   app.UseBlazorModules();
await   app.RunAsync();
