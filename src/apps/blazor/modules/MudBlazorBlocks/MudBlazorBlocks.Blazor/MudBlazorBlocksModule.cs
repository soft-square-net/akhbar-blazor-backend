using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction.BlockNode.Services;
using FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Auth;
using FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Components.Abstractions;
using FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Components.Blocks.Inputs.Custome;
using FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Layout;
using FSH.Starter.BlazorShared;
using FSH.Starter.Shared.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
// using static MudBlazor.CategoryTypes;

namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor;

public class MudBlazorBlocksModule : BlazorModuleBase
{
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicConstructors, typeof(MudBlazorBlocksModule))]
    public MudBlazorBlocksModule(ILogger logger) : base(logger)
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
        builder.Configuration.AddJsonFile($"{Constants._content}/MudBlazorBlocksModuleSettings.json", optional: true, reloadOnChange: true);
        services.Configure<MudBlazorBlocksSettings>(builder.Configuration.GetSection(MudBlazorBlocksSettings.SectionName));
        _logger.LogInformation("Configuring MudBlazorBlocks Blazor Module...");
        FshPermissions.Instance.LoadPermisions(Permissions.ToArray());

        /// Use MediatR on the Client Browser for MudBlazorBlocks Module
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(MudBlazorBlocksModule).Assembly));

        // Register Custom InputsType, the base inputs are registered already
        InputRegistry.RegisterComponent("date", typeof(DateInput));
        services.AddScoped<IRemoteValidationService, RemoteValidationService>();
        
        
        return base.ConfigureModule(services, builder);
    }

    public override async Task<WebAssemblyHost> UseModuleAsync(WebAssemblyHost app)
    {
        
        return await base.UseModuleAsync(app);
    }
}
