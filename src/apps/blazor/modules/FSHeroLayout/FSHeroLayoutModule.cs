
using System.Diagnostics.CodeAnalysis;
using FSH.Starter.Blazor.Modules.FSHeroLayout.Blazor.Auth;
using FSH.Starter.Blazor.Modules.FSHeroLayout.Blazor.Layout;
using FSH.Starter.Blazor.Modules.FSHeroLayout.Blazor.Layout.Menu;
using FSH.Starter.BlazorShared;
using FSH.Starter.BlazorShared.interfaces;
using FSH.Starter.Shared.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.Blazor.Modules.FSHeroLayout.Blazor;
public sealed class FSHeroLayoutModule : BlazorModuleBase
{
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicConstructors, typeof(FSHeroLayoutModule))]
    public FSHeroLayoutModule(ILogger logger):base(logger)
    {
        _isLayoutModule = true;
        _name = Constants.ModuleName;
        _description = $"Manage {Constants.ModuleDisplayName}, Layout for the client admin";
        _permissions = [.. ModulePermissions.All];
        _moduleMenu = new NavMenu();
    }
    public async Task InitializeAsync()
    {
        _enabled = true;
        _loaded = true;
       _initialized = true;
        await base.InitializeAsync();
    }

    public override Task ConfigureModule(IServiceCollection services, WebAssemblyHostBuilder builder)
    {
        base.ConfigureModule(services, builder);
        // Console.WriteLine(value: $@"Configuring {Name} Blazor Module...");
        // services.AddScoped<IFileExplorerStateService, FileExplorerStateService>();
        _logger.LogInformation("Configuring FSHeroLayout Blazor Module...");
        FshPermissions.Instance.LoadPermisions(Permissions.ToArray());
        return Task.CompletedTask;
    }

    public async Task<WebAssemblyHost> UseModuleAsync(WebAssemblyHost app)
    {
        
        return await base.UseModuleAsync(app);
    }
}
