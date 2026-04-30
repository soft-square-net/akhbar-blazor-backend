using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elsa.Studio.Contracts;
using Elsa.Studio.Core.BlazorWasm.Extensions;
using Elsa.Studio.Dashboard.Extensions;
using Elsa.Studio.Extensions;
using Elsa.Studio.Login.BlazorWasm.Extensions;
using Elsa.Studio.Login.Extensions;
using Elsa.Studio.Models;
using Elsa.Studio.Shell.Extensions;
using Elsa.Studio.Workflows.Designer.Extensions;
using Elsa.Studio.Workflows.Extensions;
using FSH.Starter.Blazor.Modules.ElsaWorkflow.Blazor.Layout;
using FSH.Starter.Blazor.Modules.ElsaWorkflow.Blazor.Auth;
using FSH.Starter.Blazor.Modules.ElsaWorkflow.Blazor.Layout;
using FSH.Starter.BlazorShared;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.Blazor.Modules.ElsaWorkflow.Blazor
{
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

            builder.RootComponents.RegisterCustomElsaStudioElements();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            // Register shell services and modules.
            var backendApiConfig = new BackendApiConfig
            {
                ConfigureBackendOptions = backend => {
                    backend.Url = new Uri(builder.Configuration["Elsa:Server:BaseUrl"]!);

                },
                ConfigureHttpClientBuilder = options =>
                {
                    options.BaseAddress = new Uri(builder.Configuration["Elsa:Server:BaseUrl"]!);
                    options.AuthenticationHandler = typeof(CustomAuthenticationHandler);
                    options.ApiKey = builder.Configuration["Elsa:Server:ApiKey"];
                }

            };

            builder.Services.AddCore();
            builder.Services.AddShell();
            builder.Services.AddRemoteBackend(backendApiConfig);
            builder.Services.AddLoginModule().UseElsaIdentity();
            builder.Services.AddDashboardModule();
            builder.Services.AddWorkflowsModule();
            builder.Services.UseElsaIdentity();
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
}
