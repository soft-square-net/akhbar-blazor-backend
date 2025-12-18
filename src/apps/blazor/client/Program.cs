using FSH.Starter.Blazor.Client;
using FSH.Starter.Blazor.Infrastructure;
using FSH.Starter.Blazor.Modules;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Services;
using Microsoft.Extensions.Logging;


var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Logging.SetMinimumLevel(LogLevel.Information);


await builder.Services.ConfigureBlazorModules();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddClientServices(builder.Configuration);


var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();

logger.LogInformation("Logging an Information message from Program.cs");

app.UseBlazorModules();

await   app.RunAsync();
