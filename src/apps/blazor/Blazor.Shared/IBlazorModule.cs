
using FSH.Starter.BlazorShared.interfaces;
using FSH.Starter.Shared.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MudBlazor;

namespace FSH.Starter.BlazorShared;
public interface IBlazorModule
{
    ILogger Logger { get; }
    Type StartupComponent{ get; }
    string Name { get; }
    string Description { get; }
    bool IsEnabled { get; } 
    bool IsLoaded { get; }
    static readonly bool IsLayoutModule;
    bool IsInitialized { get;}

    IModuleMenu ModuleMenu { get; }
    List<FshPermission> Permissions { get; }
    Task InitializeAsync();
    Task ConfigureModule(IServiceCollection services, WebAssemblyHostBuilder builder);
    Task<WebAssemblyHost> UseModuleAsync(WebAssemblyHost app);
    void SetStartupComponent<T>(T component) where T:ComponentBase;
}
