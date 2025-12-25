
using System.Diagnostics.CodeAnalysis;
using FSH.Starter.BlazorShared;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.Blazor.Modules.Document.Blazor;
public class DocumentModule : IBlazorModule
{
    private readonly ILogger _logger;

    public string Name => "Documents";

    public string Description => "Manage Documents, Files, Images ...etc in Varies of storage systems like AmazonS3, Local file system";

    public bool IsEnabled { get; set; } = false;
    public bool IsLoaded { get; set; } = false;
    public bool IsInitialized { get; set; } = false;

    public DocumentModule()
    {

    }
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicConstructors, typeof(DocumentModule))]

    public DocumentModule(ILogger logger)
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
            _logger.LogInformation(Description);
        }
        return Task.CompletedTask;
    }

    public Task ConfigureModule(IServiceCollection services)
    {
        Console.WriteLine("Configuring Document Blazor Module...");
        _logger.LogInformation("Configuring Document Blazor Module...");
        
        return Task.CompletedTask;
    }

    public async Task<WebAssemblyHost> UseModuleAsync(WebAssemblyHost app)
    {
        _logger.LogWarning("Using Document Module ^^^^^ ");
        return await Task.FromResult(app);
    }
}
