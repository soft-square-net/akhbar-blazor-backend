
using FSH.Starter.BlazorShared.interfaces;
using FSH.Starter.Shared.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.BlazorShared;
public interface IBlazorModule
{
    string Name { get; }
    string Description { get; }
    bool IsEnabled { get; set; } 
    bool IsLoaded { get; set; }
    bool IsInitialized { get; set; }

    IModuleMenu ModuleMenu { get; }

    Task InitializeAsync();
    Task ConfigureModule(IServiceCollection services );
    List<FshPermission> Permissions { get; }
    Task<WebAssemblyHost> UseModuleAsync(WebAssemblyHost app);
}
