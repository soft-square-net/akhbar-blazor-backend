using FSH.Starter.Blazor.Client;
using FSH.Starter.Blazor.Client.Layout;
using FSH.Starter.Blazor.Infrastructure;
using FSH.Starter.Blazor.Infrastructure.Localization;
using FSH.Starter.Blazor.Modules;
using FSH.Starter.BlazorShared.Layout;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;


var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Logging.SetMinimumLevel(LogLevel.Information);


await builder.BeforeAddingAppComponents();
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.RootComponents.Add<PageAfterScripts>("#custom-scripts");
await builder.AfterAddingAppComponents();

/* builder.Services.AddClientServices(builder.Configuration); */

await builder.BrforeBuildApp();
  var app = builder.Build();
 await app.AftereBuildApp();
/*
 var logger = app.Services.GetRequiredService<ILogger<Program>>();
 logger.LogInformation("Logging an Information message from Program.cs");
 */

await app.UseFSHLocalization();

// await app.UseModules();

await app.BeforeRunApp();
await   app.RunAsync();
await app.AftereBuildApp();
