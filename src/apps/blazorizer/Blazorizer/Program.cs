using System.Globalization;
using System.Text.Json;
using AdminDashboard;
using Blazored.LocalStorage;
using Blazorizer;
using BlazorModularEngine.Core.Hooks;
using BlazorModularEngine.Core.Models;
using BlazorModularEngine.Core.Services;
using BlazorModularEngine.Core.Services.Interfaces;
using BlazorModularEngine.Core.UI;
using BlazorModularEngine.Core.UI.Interfaces;
using BlazorModularEngine.Core.UI.Layouts;
using BlazorModularEngine.Core.UI.Menu;
using BlazorModularEngine.Core.UI.Theme;
using FSH.Starter.Blazor.Infrastructure.Api;
using FSH.Starter.Blazor.Infrastructure.Auth;
using FSH.Starter.Blazor.Infrastructure.Auth.Jwt;
using FSH.Starter.Blazor.Infrastructure.Localization;
using FSH.Starter.Blazor.Infrastructure.Notifications;
using FSH.Starter.Blazor.Infrastructure.Notifications;
using FSH.Starter.Blazor.Infrastructure.Preferences;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazorLayout;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register standard HTTP client for WASM environment
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// CRITICAL FIX: Expose IServiceCollection instance to DI container for BlazorLoaderEngine fallback
builder.Services.AddSingleton<IServiceCollection>(builder.Services);

builder.Services.AddSingleton<PluginHookManager>();
builder.Services.AddSingleton<PluginSlotRegistry>();
builder.Services.AddSingleton<DynamicServiceRegistry>();
builder.Services.AddSingleton<ModuleCacheStore>();

// 2. Theme & Layout Engine Services
builder.Services.AddSingleton<ThemeRegistry>();
builder.Services.AddSingleton<ThemeManager>();
builder.Services.AddSingleton<ILayoutManager,LayoutManager>();
builder.Services.AddSingleton<ILayoutModule, MudBlazorLayoutModule>();
builder.Services.AddSingleton<IModulesLoader, ModulesLoader>();
builder.Services.AddSingleton<IMenuRegistry, MenuRegistry>();
// 3. Loader Engine for WASM runtime assembly streaming
builder.Services.AddScoped<BlazorLoaderEngine>();

////   FSH   Services           ////////////////////
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddFSHLocalization();
builder.Services.AddAuthentication(builder.Configuration);
// Current user helper for components and services
builder.Services.AddScoped<ICurrentUserService,CurrentUserService>();
builder.Services.AddTransient<IApiClient, ApiClient>();
builder.Services.AddHttpClient("FullStackHero.API", client =>
{
    client.DefaultRequestHeaders.AcceptLanguage.Clear();
    client.DefaultRequestHeaders.AcceptLanguage.ParseAdd(CultureInfo.DefaultThreadCurrentCulture?.TwoLetterISOLanguageName);
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]!);
})
   .AddHttpMessageHandler<JwtAuthenticationHeaderHandler>()
   .Services
   .AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("FullStackHero.API"));
builder.Services.AddTransient<IClientPreferenceManager, ClientPreferenceManager>();
builder.Services.AddTransient<IPreference, ClientPreference>();
builder.Services.AddNotifications();
builder.Services.ConfigureMudBlazorLayout();


////   FSH   Services           ////////////////////

var host = builder.Build();

/////////////////////////////////////////////////////



var cacheStore = host.Services.GetRequiredService<ModuleCacheStore>();
cacheStore.StoreMetadata(new[]
{
                new ModuleMetadata
                {
                    ModuleName = "AdminDashboard",
                    Version = "1.0.0",
                    IsStartupModule = true,
                    AssemblyUrl = "_content/AdminDashboardPlugin/AdminDashboardPlugin.dll"
                },
                new ModuleMetadata
                {
                    ModuleName = "MudBlazorLayout",
                    Version = "1.0.0",
                    IsStartupModule = true,
                    AssemblyUrl = "_content/MudBlazorLayoutPlugin/MudBlazorLayoutPlugin.dll"
                }
            });

// Initialize Admin Dashboard Plugin instance for WASM client
var loaderEngine = host.Services.GetRequiredService<BlazorLoaderEngine>();
var hookManager = host.Services.GetRequiredService<PluginHookManager>();
var slotRegistry = host.Services.GetRequiredService<PluginSlotRegistry>();
var layoutManager = host.Services.GetRequiredService<ILayoutManager>();

var dynamicRegistry = host.Services.GetRequiredService<DynamicServiceRegistry>();
var runtimeAdapter = new RuntimeServiceCollectionAdapter(builder.Services, dynamicRegistry, true);

var adminModule = new AdminDashboardModule();
adminModule.ConfigureServices(runtimeAdapter);
adminModule.RegisterHooks(hookManager);
adminModule.RegisterUI(slotRegistry);

cacheStore.StoreInstance("AdminDashboard", adminModule);

var mudLayoutModule = new MudBlazorLayoutModule();
mudLayoutModule.RegisterHooks(hookManager);
mudLayoutModule.RegisterUI(slotRegistry);
cacheStore.StoreInstance("MudBlazorLayout", mudLayoutModule);

await layoutManager.SetActiveLayoutAsync(mudLayoutModule.LayoutId);

await host.RunAsync();
