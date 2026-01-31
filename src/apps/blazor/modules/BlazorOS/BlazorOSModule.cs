

using System.Diagnostics.CodeAnalysis;
using FSH.Starter.Blazor.Modules.BlazorOS.Auth;
using FSH.Starter.Blazor.Modules.BlazorOS.Layout;
using FSH.Starter.Blazor.Modules.BlazorOS.Services;
using FSH.Starter.Blazor.Modules.BlazorOS.Services.Interfaces;
using FSH.Starter.BlazorShared;
using FSH.Starter.Shared.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.Blazor.Modules.BlazorOS;
public class BlazorOSModule : BlazorModuleBase
{
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicConstructors, typeof(BlazorOSModule))]
    public BlazorOSModule(ILogger logger) : base(logger)
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
        builder.Configuration.AddJsonFile($"{Constants._content}/{Constants.ModuleName}Settings.json", optional: true, reloadOnChange: true);
        services.Configure<BlazorOSSettings>(builder.Configuration.GetSection(BlazorOSSettings.SectionName));
        _logger.LogInformation("Configuring BlazorOS Module...");
        FshPermissions.Instance.LoadPermisions(Permissions.ToArray());

        /// Use MediatR on the Client Browser for DesktopLayout Module
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(BlazorOSModule).Assembly));
        services.AddScoped<IBlazorOSAppManagerService, BlazorOSAppManagerService>();


        return base.ConfigureModule(services, builder);
    }

    public async Task<WebAssemblyHost> UseModuleAsync(WebAssemblyHost app)
    {

        return await base.UseModuleAsync(app);
    }
}
