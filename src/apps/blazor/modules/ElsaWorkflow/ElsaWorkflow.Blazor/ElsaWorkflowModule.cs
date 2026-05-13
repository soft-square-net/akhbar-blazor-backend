using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorMonaco;
using Elsa.Studio.Contracts;
using Elsa.Studio.Core.BlazorWasm.Extensions;
using Elsa.Studio.DomInterop.Extensions;
using Elsa.Studio.Extensions;
using Elsa.Studio.Host.CustomElements.Components;
using Elsa.Studio.Host.CustomElements.HttpMessageHandlers;
using Elsa.Studio.Host.CustomElements.Services;
using Elsa.Studio.Login.BlazorWasm.Extensions;
using Elsa.Studio.Login.Extensions;
using Elsa.Studio.Models;
using Elsa.Studio.Shell.Extensions;
using Elsa.Studio.Workflows.Designer.Extensions;
using Elsa.Studio.Workflows.Extensions;
using FSH.Starter.Blazor.Modules.ElsaWorkflow.Blazor.Auth;
using FSH.Starter.Blazor.Modules.ElsaWorkflow.Blazor.Layout;
using FSH.Starter.BlazorShared;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using MudExtensions.Services;
using Microsoft.Extensions.Configuration;
namespace FSH.Starter.Blazor.Modules.ElsaWorkflow.Blazor;

public class ElsaWorkflowModule : BlazorModuleBase
{
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicConstructors, typeof(ElsaWorkflowModule))]
    public ElsaWorkflowModule(ILogger logger) : base(logger)
    {
        _isLayoutModule = false;
        _name = "Elsa";
        _description = "Module for workflow management using Elsa.";
        _permissions = [.. ModulePermissions.All];
        _moduleMenu = new NavMenu();
        // SetStartupComponent(new List());
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
    }

    public override Task ConfigureModule(IServiceCollection services, WebAssemblyHostBuilder builder)
    {

        // builder.RootComponents.RegisterCustomElsaStudioElements();

        // Register the custom elements.
        builder.RootComponents.RegisterCustomElsaStudioElements();
        builder.RootComponents.RegisterCustomElement<BackendProvider>("elsa-backend-provider");
        builder.RootComponents.RegisterCustomElement<WorkflowDefinitionEditorWrapper>("elsa-workflow-definition-editor");
        builder.RootComponents.RegisterCustomElement<WorkflowInstanceViewerWrapper>("elsa-workflow-instance-viewer");
        builder.RootComponents.RegisterCustomElement<WorkflowInstanceListWrapper>("elsa-workflow-instance-list");
        builder.RootComponents.RegisterCustomElement<WorkflowDefinitionListWrapper>("elsa-workflow-definition-list");
        // Register local services.
        builder.Services.AddSingleton<BackendService>();

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        // Register shell services and modules.
        var backendApiConfig = new BackendApiConfig
        {
            ConfigureBackendOptions = backend =>
            {
                backend.Url = new Uri(builder.Configuration["Elsa:Server:BaseUrl"]!);

            },
            ConfigureHttpClientBuilder = options =>
            {
                options.BaseAddress = new Uri(builder.Configuration["Elsa:Server:BaseUrl"]!);
                // options.AuthenticationHandler = typeof(CustomAuthenticationHandler);
                // options.ApiKey = builder.Configuration["Elsa:Server:ApiKey"];
                // options.AuthenticationHandler = typeof(AuthHttpMessageHandler);
            }

        };

        //// Register the modules.
        //var backendApiConfig2 = new BackendApiConfig
        //{
        //    ConfigureBackendOptions = options => builder.Configuration.GetSection("Backend").Bind(options),
        //    ConfigureHttpClientBuilder = options =>
        //    {
        //        options.ApiKey = builder.Configuration["Backend:ApiKey"];
        //        options.AuthenticationHandler = typeof(AuthHttpMessageHandler);
        //    },
        //};

        builder.Services.AddCore();
        // builder.Services.AddShell();
        builder.Services.AddShell(x => x.DisableAuthorization = true);
        builder.Services.AddRemoteBackend(backendApiConfig);
        // builder.Services.Replace(ServiceDescriptor.Scoped<IRemoteBackendAccessor, ComponentRemoteBackendAccessor>());
        builder.Services.AddLoginModule();// .UseElsaIdentity();



        // 2. Register it in Program.cs right after AddElsaStudioCore()

        builder.Services.TryAddScoped<IAuthenticationProviderManager, CustomElementsAuthenticationProviderManager>();

        // builder.Services.AddDashboardModule();
        builder.Services.AddWorkflowsModule();
        // builder.Services.UseElsaIdentity();
        builder.Services.AddMudExtensions();
        return base.ConfigureModule(services, builder);
    }

    public async Task<WebAssemblyHost> UseModuleAsync(WebAssemblyHost app)
    {
        // Map Studio UI
        // app.MapElsaStudio("/workflows");
        // Run each startup task.

        var startupTaskRunner = app.Services.GetRequiredService<IStartupTaskRunner>();
        await startupTaskRunner.RunStartupTasksAsync();
        return await base.UseModuleAsync(app);
    }
}

// 1. Create a minimal token provider that reads from the custom element attributes
public class CustomElementsAuthenticationProviderManager : IAuthenticationProviderManager
{
    public Task<string?> GetAuthenticationTokenAsync(string? tokenName, CancellationToken cancellationToken = default)
    {
        return GetTokenAsync(cancellationToken).AsTask();
    }

    // Provide an empty or custom token retrieval logic if your elements supply tokens
    public ValueTask<string?> GetTokenAsync(CancellationToken cancellationToken = default)
        => ValueTask.FromResult<string?>(null);

    public ValueTask SetTokenAsync(string? token, CancellationToken cancellationToken = default)
        => ValueTask.CompletedTask;
}
