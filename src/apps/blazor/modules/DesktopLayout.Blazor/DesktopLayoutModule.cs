
using System.Diagnostics.CodeAnalysis;
using System.Security;
using System.Xml.Linq;
using FSH.Starter.Blazor.Modules.DesktopLayout.Blazor.Auth;
using FSH.Starter.Blazor.Modules.DesktopLayout.Blazor.Layout;
using FSH.Starter.BlazorShared;
using FSH.Starter.BlazorShared.interfaces;
using FSH.Starter.Shared.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.Blazor.Modules.DesktopLayout.Blazor;
public class DesktopLayoutModule : BlazorModuleBase
{
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicConstructors, typeof(DesktopLayoutModule))]
    public DesktopLayoutModule(ILogger logger) : base(logger)
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

    public override Task ConfigureModule(IServiceCollection services)
    {

        return base.ConfigureModule(services);
    }

    public async Task<WebAssemblyHost> UseModuleAsync(WebAssemblyHost app)
    {

        return await base.UseModuleAsync(app);
    }
}
