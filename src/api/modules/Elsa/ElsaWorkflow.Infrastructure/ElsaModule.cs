

using System;
using Acornima.Ast;
using AspNetCore.Authentication.ApiKey;
using Carter;
using Elsa;
using Elsa.Common.Multitenancy;
using Elsa.EntityFrameworkCore;
using Elsa.EntityFrameworkCore.Extensions;
using Elsa.EntityFrameworkCore.Modules.Identity;
using Elsa.EntityFrameworkCore.Modules.Management;
using Elsa.EntityFrameworkCore.Modules.Runtime;
using Elsa.EntityFrameworkCore.Modules.Tenants;
using Elsa.Extensions;
using Elsa.Identity.Multitenancy;
using Elsa.Tenants.Extensions;
using Elsa.Workflows.LogPersistence.Strategies;
using Elsa.Workflows.Management.Features;
using Elsa.Workflows.Runtime.Options;
using ElsaWorkflow.Infrastructure.Services;
using FastEndpoints;
using FastEndpoints.Swagger;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Infrastructure.Persistence;
using FSH.Framework.Infrastructure.Tenant;
using FSH.Starter.ElsaWorkflow.Infrastructure.Auth;
using FSH.Starter.ElsaWorkflow.Infrastructure.Auth.ApiKey;
using FSH.Starter.ElsaWorkflow.Infrastructure.Endpoints;
using FSH.Starter.ElsaWorkflow.Infrastructure.Persistence;
using FSH.Starter.ElsaWorkflow.Infrastructure.Tenants;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FSH.Starter.WebApi.ElsaWorkflow.Infrastructure;

public static class ElsaModule
{
    public class Endpoints : ICarterModule
    {
        public Endpoints() { }
        //    : base("Elsa")
        //{
        //    this.WithName("Elsa");
        //    this.WithGroupName("Elsa");
        //    this.WithDisplayName("Elsa Workflow");
        //    this.WithDescription("This moudule scope focus on hadeling files distribution throw the framework, inside local folders or even on the cloud Like AWS Buckets");
        //    this.WithSummary("Files distribution throw the framework");
        //}
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            //// 1. Create your module group
            //var charterGroup = app.MapGroup("/elsa");

            //// 2. Map the Elsa API endpoints to this group
            //// This makes endpoints available at /elsa/api/...
            //charterGroup.MapWorkflowsApi();

            //// 3. Optional: Add a simple health check for your module
            //charterGroup.MapGet("/status", () => "Charter Module Active");
        }


    }
    public static WebApplicationBuilder RegisterElsaServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.BindDbContext<ElsaStoreDbContext>();
        builder.Services.AddRazorPages();
        //builder.Services.AddDbContext<ElsaStoreDbContext>(ef =>
        //{
        //    ef.UseNpgsql(builder.Configuration.GetSection(nameof(DatabaseOptions)).Get<DatabaseOptions>()?.ConnectionString
        //        ?? throw new InvalidOperationException($"Failed to bind {nameof(DatabaseOptions)} from configuration.")
        //    );
        //});



        // Register API Key Store
        // builder.Services.AddSingleton<IApiKeyStore, InMemoryApiKeyStore>();

        //// Configure authentication
        // builder.Services.AddAuthentication("ApiKey")
        //    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("ApiKey", null);

        // register elsa tenant service
        // builder.Services.AddScoped<ITenantService, ElsaTenantService>();

        DatabaseOptions? dbConfig = builder.Configuration.GetSection(nameof(DatabaseOptions)).Get<DatabaseOptions>();
        if (dbConfig is null || string.IsNullOrEmpty(dbConfig.ConnectionString))
            throw new InvalidOperationException($"Failed to bind {nameof(DatabaseOptions)} from configuration.");




#pragma warning disable S125 // Sections of code should not be commented out
        // Action<PersistenceFeatureBase<EFCoreIdentityPersistenceFeature, IdentityElsaDbContext>> usePgresSQL = ef => ef.UsePostgreSql(dbConfig.ConnectionString);
        // Define your custom scheme name
        string customSchemeName = "elsaBearer";
        //// Disable endpoint security
        
        
        // Elsa.EndpointSecurityOptions.DisableSecurity();
        // ⚠️ DEVELOPMENT ONLY: Disable all endpoint security

        builder.Services.AddElsa(elsa =>
        {
            // Inside Program.cs
            elsa.AddSwagger();
            // builder.Services.AddFastEndpoints();
            // builder.Services.SwaggerDocument();
            // Default Identity features for authentication/authorization.
            //elsa.UseIdentity(identity =>
            //{
            //    // identity.UseAdminUserProvider();

            //    identity.TokenOptions = options =>
            //    {
            //        options.SigningKey = builder.Configuration["JwtOptions:Key"] ?? "";
            //        options.AccessTokenLifetime = TimeSpan.FromMinutes(10);
            //        options.RefreshTokenLifetime = TimeSpan.FromDays(7);
            //    };
            // identity.UseEntityFrameworkCore(ef =>
            // {
            //     ef.UsePostgreSql(dbConfig.ConnectionString);
            //     // ef.RunMigrations = true; 
            // });
            //});


            // Configure ASP.NET authentication/authorization.
            // elsa.UseDefaultAuthentication(auth => auth.UseAdminApiKey());

            //elsa.UseServerAuthentication(e => { 

            //});
            elsa.UseIdentity(identity =>
            {
                identity.UseAdminUserProvider();
                identity.TokenOptions = options =>
                {
                    options.SigningKey = "QsJbczCNysv/5SGh+U7sxedX8C07TPQPBdsnSDKZ/aE=";
                    options.AccessTokenLifetime = TimeSpan.FromMinutes(10);
                    options.RefreshTokenLifetime = TimeSpan.FromDays(7);
                };
            });

            /**********************************************************************
            builder.Services.AddScoped<ITenantsProvider, FinbuckleTenantProvider>();
            elsa.UseTenants(tenants =>
            {
                tenants.ConfigureMultitenancy(options =>
                    options.TenantResolverPipelineBuilder.Append<FinbuckleTenantResolver>());

                tenants.UseTenantManagement(options => { 
                    options.UseEntityFrameworkCore(ef =>
                    {
                        ef.UsePostgreSql(dbConfig.ConnectionString);
                        ef.RunMigrations = true;
                    });
                });

                //tenants.UseConfigurationBasedTenantsProvider(e =>
                //{
                //    var defaultTnnt = e.Tenants.FirstOrDefault(e => e.Name == "root");
                //    e.IsEnabled = true;
                //});

                //// 1. Tell Elsa to use your custom Finbuckle-based provider
                // This replaces UseConfigurationBasedTenantsProvider()
                //// 2. Keep your resolver from before to handle "Current" tenant detection
                //tenants.ConfigureMultitenancy(options =>
                //    options.TenantResolverPipelineBuilder.Append<FinbuckleTenantResolver>());

                //tenants.ConfigureMultitenancy(options =>
                //{
                //    // Configure the tenant resolution pipeline.
                //    options.TenantResolverPipelineBuilder.Append<ClaimsTenantResolver>();
                //});

                // tenants.UseConfigurationBasedTenantsProvider(options => builder.Configuration.GetSection("Multitenancy").Bind(options));

            }); // Enable multi-tenancy support.
            ****************************************************************************/
            // Configure Management layer to use EF Core.
            elsa.UseWorkflowManagement(management => {
                management.UseEntityFrameworkCore(ef =>
                {
                    // ef.UsePostgreSql<WorkflowManagementPersistenceFeature, ManagementElsaDbContext>(dbConfig.ConnectionString); 
                    ef.UsePostgreSql(dbConfig.ConnectionString);

                    // ef.RunMigrations = true;
                });
            });

            // Configure Runtime layer to use EF Core.
            elsa.UseWorkflowRuntime(runtime => {
                runtime.UseEntityFrameworkCore(ef =>
                    {
                        ef.UsePostgreSql(dbConfig.ConnectionString);
                        // ef.RunMigrations = true;
                    });
                runtime.WorkflowInboxCleanupOptions = options =>
                {
                    // Clean up completed workflow instances after 30 days
                    options.BatchSize = 100;
                    options.SweepInterval = TimeSpan.FromMinutes(60);
                };
            });


            // Expose Elsa API endpoints.
            elsa.UseWorkflowsApi();

            // Setup a SignalR hub for real-time updates from the server.
            elsa.UseRealTimeWorkflows();

            // Enable C# workflow expressions
            elsa.UseCSharp();

            // Enable JavaScript workflow expressions
            elsa.UseJavaScript((Elsa.JavaScript.Options.JintOptions options) => options.AllowClrAccess = true);

            // Enable HTTP activities.
            elsa.UseHttp(options => options.ConfigureHttpOptions = httpOptions => httpOptions.BaseUrl = new("https://localhost:5001"));

            // Use timer activities.
            elsa.UseScheduling();

            // Register custom activities from the application, if any.
            elsa.AddActivitiesFrom<Program>();

            // Register custom workflows from the application, if any.
            elsa.AddWorkflowsFrom<Program>();

            // Inside Program.cs
            // elsa.AddSwagger();
        });

        builder.Services.AddCors(cors => cors.AddDefaultPolicy(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders("*")));
        builder.Services.AddRazorPages(options => options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute()));

        // 2. Configure Custom Authentication
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = "CustomBearer"; // or IdentityConstants.ApplicationScheme
            options.DefaultChallengeScheme = "CustomBearer";
        })
        // Custom API JWT
        .AddJwtBearer("CustomBearer", options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                // ... Validations
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:Key"] ?? "")),
                ValidIssuers = new[] { "elsa-issuer", "elsa-workflows", "https://fullstackhero.net", "elsa-api" },
                ValidAudiences = new[] { "elsa-audience", "fullstackhero", "elsa-workflows", "elsa-api" },
                IssuerSigningKeyResolver = (token, securityToken, kid, validationParameters) =>
                {
                    // Implement your logic to retrieve the signing key based on the 'kid' or other token information.
                    // For example, you could look up the key from a database or configuration.
                    // Here, we return a single key for simplicity.
                    return new[] { new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:Key"] ?? "")) };
                }
            };
        });
#pragma warning restore S125 // Sections of code should not be commented out

        // Configure CORS to allow designer app hosted on a different origin to invoke the APIs.
        builder.Services.AddCors(cors => cors
            .AddDefaultPolicy(policy => policy
                .AllowAnyOrigin() // For demo purposes only. Use a specific origin instead.
                .AllowAnyHeader()
                .AllowAnyMethod()
                .WithExposedHeaders("x-elsa-workflow-instance-id"))); // Required for Elsa Studio in order to support running workflows from the designer. Alternatively, you can use the `*` wildcard to expose all headers.

        return builder;
    }
    public static async Task<WebApplication> UseElsaModule(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        if (app.Environment.IsDevelopment())
        {
            // await app.Services.MigrateElsaDatabaseAsync();
        }
        app.UseWorkflowsApi(); // Use Elsa API endpoints.
        app.UseWorkflows(); // Use Elsa middleware to handle HTTP requests mapped to HTTP Endpoint activities.
        app.UseWorkflowsSignalRHubs(); // Optional SignalR integration. Elsa Studio uses SignalR to receive real-time updates from the server. 

        app.MapRazorPages();
        app.MapFallbackToPage("/_Host");
        return app;
    }

    private static async Task InitializeDatabase(FshTenantInfo tenant, WebApplicationBuilder builder)
    {
        // First create a new scope
        using (var scope = builder.Services.BuildServiceProvider().CreateScope())
        {
            // Then set current tenant so the right connection string is used
            scope.ServiceProvider.GetRequiredService<IMultiTenantContextSetter>()
                .MultiTenantContext = new MultiTenantContext<FshTenantInfo>()
                {
                    TenantInfo = tenant
                };

            // using the scope, perform migrations / seeding
            var initializers = scope.ServiceProvider.GetServices<IDbInitializer>();
            foreach (var initializer in initializers)
            {
                await initializer.MigrateAsync(CancellationToken.None).ConfigureAwait(false);
                await initializer.SeedAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }
    }

    private static async Task<IEnumerable<Tenant>> GetTenants(WebApplicationBuilder builder)
    {
        // First create a new scope
        using (var scope = builder.Services.BuildServiceProvider().CreateScope())
        {
            // Then set current tenant so the right connection string is used
            ITenantService srvTnt = scope.ServiceProvider.GetRequiredService<ITenantService>();

            return await srvTnt.ListAsync(CancellationToken.None).ConfigureAwait(false);

        }
    }
}



//identity.TokenOptions = options =>
//                {
//                    //JwtBearerOptions opt = new()
//                    //{
//                    //    ClaimsIssuer = "elsa-issuer",
//                    //    Audience = "elsa-audience",
//                    //    RequireHttpsMetadata = false, // For development purposes only. Set to true in production.
//                    //    SaveToken = true,
//                    //};
//                    //options.ConfigureJwtBearerOptions(opt);



//                    // This key needs to be at least 256 bits long.
//                    options.SigningKey = builder.Configuration["JwtOptions:Key"] ?? "";
//                    options.AccessTokenLifetime = TimeSpan.FromMinutes(int.Parse(builder.Configuration["JwtOptions:TokenExpirationInMinutes"] ?? "10"));
//                    options.RefreshTokenLifetime = TimeSpan.FromDays(int.Parse(builder.Configuration["JwtOptions:RefreshTokenExpirationInDays"] ?? "7"));
//                };
