using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using FSH.Starter.Blazor.Modules.Keyword.Blazor.Auth;
using FSH.Starter.Blazor.Modules.Keyword.Blazor.Layout;
using FSH.Starter.BlazorShared;
using FSH.Starter.Shared.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
// using static MudBlazor.CategoryTypes;

namespace FSH.Starter.Blazor.Modules.Keyword.Blazor;

public class KeywordModule : BlazorModuleBase
{
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicConstructors, typeof(KeywordModule))]
    public KeywordModule(ILogger logger) : base(logger)
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
        builder.Configuration.AddJsonFile($"{Constants._content}/KeywordModuleSettings.json", optional: true, reloadOnChange: true);
        services.Configure<KeywordSettings>(builder.Configuration.GetSection(KeywordSettings.SectionName));
        _logger.LogInformation("Configuring Keyword Blazor Module...");
        FshPermissions.Instance.LoadPermisions(Permissions.ToArray());

        /// Use MediatR on the Client Browser for Keyword Module
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(KeywordModule).Assembly));


        return base.ConfigureModule(services, builder);
    }

    public override async Task<WebAssemblyHost> UseModuleAsync(WebAssemblyHost app)
    {

        return await base.UseModuleAsync(app);
    }
}
