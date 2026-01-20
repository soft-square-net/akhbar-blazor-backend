
using System.Diagnostics.CodeAnalysis;
using FSH.Starter.Blazor.Modules.DesktopLayout.Blazor.Auth;
using FSH.Starter.Blazor.Modules.DesktopLayout.Blazor.Layout;
using FSH.Starter.BlazorShared;
using FSH.Starter.BlazorShared.interfaces;
using FSH.Starter.Shared.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.Blazor.Modules.DesktopLayout.Blazor;
public class DesktopLayoutModule : IBlazorModule
{
    private readonly ILogger _logger;

    public string Name => Constants.ModuleName;

    public string Description => $"Manage {Constants.ModuleDisplayName}, Layout for the client admin";

    public bool IsEnabled { get; set; }
    public bool IsLoaded { get; set; }
    public bool IsInitialized { get; set; }

    public List<FshPermission> Permissions => [.. ModulePermissions.All];

    public IModuleMenu ModuleMenu => new NavMenu();


    //public DocumentModule()
    //{

    //}
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicConstructors, typeof(DesktopLayoutModule))]

    public DesktopLayoutModule(ILogger logger)
    {
        _logger = logger;
    }
    public Task InitializeAsync()
    {
        this.IsEnabled = true;
        this.IsLoaded = true;
        this.IsInitialized = true;
        if (_logger is not null)
        {
            _logger.LogInformation("Blazor Module {ModuleName} is initialized.", Name);
            _logger.LogInformation("{Description}", Description);
        }
        return Task.CompletedTask;
    }

    public Task ConfigureModule(IServiceCollection services)
    {
        // Console.WriteLine(value: $@"Configuring {Name} Blazor Module...");
        // services.AddScoped<IFileExplorerStateService, FileExplorerStateService>();
        _logger.LogInformation("Configuring FSHeroLayout Blazor Module...");
        FshPermissions.Instance.LoadPermisions(Permissions.ToArray());
        return Task.CompletedTask;
    }


    public async Task<WebAssemblyHost> UseModuleAsync(WebAssemblyHost app)
    {
        _logger.LogWarning("Using FSHeroLayout Module ^^^^^ ");
        return await Task.FromResult(app);
    }
}
