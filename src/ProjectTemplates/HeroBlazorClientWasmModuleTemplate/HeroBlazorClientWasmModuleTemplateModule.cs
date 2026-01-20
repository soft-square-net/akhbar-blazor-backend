
using FSH.Starter.Blazor.Modules.HeroBlazorClientWasmModuleTemplate.Auth;
using FSH.Starter.Blazor.Modules.HeroBlazorClientWasmModuleTemplate.Layout;
using FSH.Starter.BlazorShared;
using FSH.Starter.BlazorShared.interfaces;
using FSH.Starter.Shared.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace FSH.Starter.Blazor.Modules.HeroBlazorClientWasmModuleTemplate;
public class HeroBlazorClientWasmModuleTemplateModule : IBlazorModule
{
    public string Name => "HeroBlazorClientWasmModuleTemplate";

    public string Description => $"Write HeroBlazorClientWasmModuleTemplate Module Description Here";

    public bool IsEnabled { get; set; }
    public bool IsLoaded { get; set; }
    public bool IsInitialized { get; set; }

    public IModuleMenu ModuleMenu => new NavMenu();

    public List<FshPermission> Permissions => [.. ModulePermissions.All];

    public Task ConfigureModule(Microsoft.Extensions.DependencyInjection.IServiceCollection services)
    {
        throw new NotImplementedException();
    }

    public Task InitializeAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<WebAssemblyHost> UseModuleAsync(WebAssemblyHost app)
    {
        await Task.CompletedTask;
        return app;
    }
}
