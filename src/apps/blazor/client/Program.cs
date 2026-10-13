using System.Text.Json;
using FSH.Starter.Blazor.Client;
using FSH.Starter.Blazor.Client.Layout;
using FSH.Starter.Blazor.Infrastructure;
using FSH.Starter.Blazor.Infrastructure.Api;
using FSH.Starter.Blazor.Infrastructure.Localization;
using FSH.Starter.Blazor.Modules;
using FSH.Starter.Blazor.Modules.Plugins;
using FSH.Starter.BlazorShared.Layout;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FSH.Starter.Blazor.Modules.Services;

    var builder = WebAssemblyHostBuilder.CreateDefault(args);
    
    builder.Logging.SetMinimumLevel(LogLevel.Information);
    using var temporaryProvider = builder.Services.BuildServiceProvider();
    var logger = temporaryProvider.GetRequiredService<ILogger<Program>>();
    
    var _apiClient = new ApiClient(
        new HttpClient { BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiBaseUrl")) });
    builder.Services.AddScoped<IApiClient>( s => _apiClient );
    builder.Services.AddScoped<ILogger>( s => logger);
    // # 1. ApiClient Load Plugins Meta from the api Server with assemblydata if Startup Plugin
    // * 1.1 Load Plugins info from the api Server

    var PluginResponse = await _apiClient.SearchPluginsEndpointAsync(
        new SearchPluginsCommand()
        {
            PageNumber = 0,
            PageSize = 10
        });
        
    // * 1.2 Define your runtime variables in a Dictionary 
    var customSettings = new Dictionary<string, string>
    {
        { "Plugins", JsonSerializer.Serialize(PluginResponse.Items) }
    };
    // 2. Add Plugins to Configurations
    builder.Configuration.AddInMemoryCollection(customSettings);

    await builder.Services.ConfigureBlazorModules(builder); 
    builder.Services.AddClientServices(builder.Configuration); 
    

    builder.RootComponents.Add<App>("#app");
    builder.RootComponents.Add<HeadOutlet>("head::after");


    builder.Services.AddScoped<PluginManager>(s => new PluginManager(_apiClient, builder, logger));

    var app = builder.Build();
    
    
    await app.UseFSHLocalization();
    await app.UseModules();
    await app.UsePluginsAsync();
    
    
    await   app.RunAsync();
