
using System.Diagnostics.CodeAnalysis;
using FSH.Starter.Blazor.Modules.Document.Blazor.Auth;
using FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Services;
using FSH.Starter.Blazor.Modules.Document.Blazor.Layout;
using FSH.Starter.BlazorShared;
using FSH.Starter.BlazorShared.interfaces;
using FSH.Starter.Shared.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.Blazor.Modules.Document.Blazor;
public sealed class DocumentModule : BlazorModuleBase
{
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicConstructors, typeof(DocumentModule))]
    public DocumentModule(ILogger logger):base(logger)
    {
        _isLayoutModule = false;
        _name = "Documents";
        _description = "Module for document management and file exploration.";
        _permissions = [.. ModulePermissions.All];
        _moduleMenu = new NavMenu();
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
    }

    public override Task ConfigureModule(IServiceCollection services)
    {
        // Console.WriteLine(value: $@"Configuring {Name} Blazor Module...");
        services.AddScoped<IFileExplorerStateService, FileExplorerStateService>();
        services.AddScoped<IFileExplorerFileActionsService, FileExplorerFileActionsService>();
        services.AddScoped<IFileExplorerFolderActionsService, FileExplorerFolderActionsService>();
        return base.ConfigureModule(services);
    }

    public async Task<WebAssemblyHost> UseModuleAsync(WebAssemblyHost app)
    {
       return await base.UseModuleAsync(app);
        // return await Task.FromResult(app);
    }
}
