
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;
using System.Security;
using System.Xml.Linq;
using FSH.Starter.Blazor.Modules.DesktopLayout.Blazor.Auth;
using FSH.Starter.Blazor.Modules.DesktopLayout.Blazor.Layout;
using FSH.Starter.Blazor.Modules.DesktopLayout.Blazor.Services;
using FSH.Starter.Blazor.Modules.DesktopLayout.Blazor.Services.Interfaces;
using FSH.Starter.BlazorShared;
using FSH.Starter.BlazorShared.interfaces;
using FSH.Starter.Shared.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MudBlazor.Extensions;

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



    public override Task ConfigureModule(IServiceCollection services, WebAssemblyHostBuilder builder)
    {
        builder.Configuration.AddJsonFile($"{Constants._content}/DesktopLayoutModuleSettings.json", optional: true, reloadOnChange: true);
        services.Configure<DesktopLayoutSettings>(builder.Configuration.GetSection(DesktopLayoutSettings.SectionName));
        _logger.LogInformation("Configuring DesktopLayout Blazor Module...");
        FshPermissions.Instance.LoadPermisions(Permissions.ToArray());

        /// Use MediatR on the Client Browser for DesktopLayout Module
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DesktopLayoutModule).Assembly));
        services.AddScoped<IDesktopAppManagerService,DesktopAppManagerService> ();


        return base.ConfigureModule(services, builder);
    }

    public async Task<WebAssemblyHost> UseModuleAsync(WebAssemblyHost app)
    {

        return await base.UseModuleAsync(app);
    }
}
