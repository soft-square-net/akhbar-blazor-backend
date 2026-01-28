using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FSH.Starter.BlazorShared.interfaces;
using FSH.Starter.Shared.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.BlazorShared;
public abstract class BlazorModuleBase : IBlazorModule
{
    ComponentBase _startupComponent;
    protected ILogger _logger { get; init; }
    protected string _name { get; init; }
    protected string _description { get; init; }
    protected bool _isLayoutModule { get; init; }
    protected List<FshPermission> _permissions { get; init; }
    protected bool _loaded { get; set; }
    protected bool _enabled { get; set; }
    protected bool _initialized { get; set; }
    protected IModuleMenu _moduleMenu { get; set; }
    public string Name => _name;
    public string Description => _description;

    public bool IsEnabled => _enabled;
    public bool IsLoaded => _loaded;

    public  bool IsLayoutModule => _isLayoutModule;
    public bool IsInitialized => _initialized;

    public IModuleMenu ModuleMenu => _moduleMenu;
    public ILogger Logger => _logger;
    public virtual List<FshPermission> Permissions => _permissions;

    public Type StartupComponent => _startupComponent.GetType();

    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicConstructors, typeof(BlazorModuleBase))]
    public BlazorModuleBase(ILogger logger)
    {
        _logger = logger;
    }

    public virtual Task ConfigureModule(IServiceCollection services, WebAssemblyHostBuilder builder)
    {
        _logger.LogInformation("Configuring Document Blazor Module...");
        FshPermissions.Instance.LoadPermisions(Permissions.ToArray());
        return Task.CompletedTask;
    }

    public async virtual Task InitializeAsync()
    {
        _loaded = true;
        _enabled = true;
        if (_logger is not null)
        {
            _logger.LogInformation("Blazor Module {ModuleName} is initialized.", Name);
            _logger.LogInformation("{Description}", Description);
        }

        _initialized = true;
    }

    public async virtual Task<WebAssemblyHost> UseModuleAsync(WebAssemblyHost app)
    {
        _logger.LogWarning("Using {Name} Module ^^^^^ ", Name);
        await Task.FromResult(0);
        return app;
    }

    public void SetStartupComponent<T>(T component) where T : ComponentBase
    {
        _startupComponent = component;
    }
}
