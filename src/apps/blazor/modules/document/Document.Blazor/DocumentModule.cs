using System;
using System.Diagnostics.CodeAnalysis;
using FSH.Starter.Blazor.Shared;
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
}
