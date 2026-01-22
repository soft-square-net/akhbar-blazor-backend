
using FSH.Starter.BlazorShared.interfaces;
using FSH.Starter.Shared.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.BlazorShared;
public interface IBlazorModule
{
    ILogger Logger { get; }
    string Name { get; }
    string Description { get; }
    bool IsEnabled { get; } 
    bool IsLoaded { get; }
    static readonly bool IsLayoutModule;
    bool IsInitialized { get;}

    IModuleMenu ModuleMenu { get; }

    Task InitializeAsync();
    Task ConfigureModule(IServiceCollection services );
    List<FshPermission> Permissions { get; }
    Task<WebAssemblyHost> UseModuleAsync(WebAssemblyHost app);
}
