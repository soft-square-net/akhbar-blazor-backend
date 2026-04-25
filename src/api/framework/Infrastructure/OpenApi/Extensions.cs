using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace FSH.Framework.Infrastructure.OpenApi;

public static class Extensions
{
    public static IServiceCollection ConfigureOpenApi(this IServiceCollection services)
    {

        ArgumentNullException.ThrowIfNull(services);
        services.AddEndpointsApiExplorer();
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
       
       
        services
            .AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.FullName);
                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                // options.OperationFilter<SwaggerDefaultValues>();
                // // options.OperationFilter<ElsaOpenApiFilter>();
                // options.DocumentFilter<DocumentModuleFilterProcessor>();
                options.SwaggerDoc("v3", new OpenApiInfo { Title = "Elsa Server", Version = "v3" });


                //// Your API documentation
                //options.SwaggerDoc("v1", new OpenApiInfo
                //{
                //    Title = "My App API",
                //    Version = "v1"
                //});

                //// Elsa API documentation
                //options.SwaggerDoc("elsa", new OpenApiInfo
                //{
                //    Title = "Elsa Workflows API",
                //    Version = "v3"
                //});

                options.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var controllerName = apiDesc.ActionDescriptor.RouteValues["controller"];
                    bool isElsa = apiDesc.RelativePath?.StartsWith("elsa", StringComparison.OrdinalIgnoreCase) ?? false;
                    if (isElsa)
                    {
                        var sss = apiDesc.RelativePath;
                    }
                    if (docName == "v3") return isElsa;

                    return !isElsa;
                });




                // DocInclusionPredicate separates endpoints based on their route or group name
                //options.DocInclusionPredicate((docName, apiDesc) =>
                //{
                //    if (docName == "elsa") return apiDesc.RelativePath.StartsWith("elsa/api");
                //    return !apiDesc.RelativePath.StartsWith("elsa/api");
                //});


                //// Your API documentation
                //options.SwaggerDoc("v1", new OpenApiInfo
                //{
                //    Title = "My App API",
                //    Version = "v1"
                //});

                //// Elsa API documentation
                //options.SwaggerDoc("elsa", new OpenApiInfo
                //{
                //    Title = "Elsa Workflows API",
                //    Version = "v1"
                //});


                /**************************************************************************************
                options.DocInclusionPredicate((docName, apiDesc) =>
                {
                    //// Exclude Elsa API endpoints from Swagger documentation
                    //var actionDescriptor = apiDesc.ActionDescriptor;
                    //var controllerName = actionDescriptor.RouteValues["controller"];

                    //if (controllerName?.StartsWith("Elsa") == true)
                    //{
                    //    return false;
                    //}

                    //return true;

                    var controllerName = apiDesc.ActionDescriptor.RouteValues["controller"];

                    if (docName == "elsa")
                    {
                        return controllerName?.StartsWith("Elsa") == true;
                    }

                    return controllerName?.StartsWith("Elsa") != true;
                    // return true;
                });
                *******************************************************************************************/

                /////////////////////////////////////////////////////////////////////////////
                options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
                        },
                        Array.Empty<string>()
                    }
                });
                //////////////////////////////////////////////////////////////////////////////////
            });
        services
            .AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.DefaultApiVersion = new ApiVersion(1);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            })
            .EnableApiVersionBinding();
        return services;
    }
    public static WebApplication UseOpenApi(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);
        if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "docker")
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.DocExpansion(DocExpansion.None);
                options.DisplayRequestDuration();

                //options.SwaggerEndpoint("/swagger/identity/swagger.json", "Identity Module");
                //options.SwaggerEndpoint("/swagger/tenants/swagger.json", "Tenants Module");
                //options.SwaggerEndpoint("/swagger/catalog/swagger.json", "Catalog Module");
                //options.SwaggerEndpoint("/swagger/documents/swagger.json", "Document Module");
                //options.SwaggerEndpoint("/swagger/todo/swagger.json", "Todo Module");
                //options.SwaggerEndpoint("/swagger/elsa/swagger.json", "Elsa Workflows API");


                /***************************************************************************************
                var DescribeApiVersions = app.DescribeApiVersions();

                var swaggerEndpoints = DescribeApiVersions.Select(desc => new
                {
                    Url = $"/swagger/{desc.GroupName}/swagger.json",
                    // Url = $"/swagger/v{desc.ApiVersion}/swagger.json",
                    Name = desc.GroupName.ToUpperInvariant()
                });
                foreach (var endpoint in swaggerEndpoints)
                {
                    options.SwaggerEndpoint(endpoint.Url, endpoint.Name);
                }

                ***********************************************************************************/
                //var swaggerEndpoints = app.DescribeApiVersions()
                //    .Select(desc => new
                //    {
                //        Url = $"../swagger/{desc.GroupName}/swagger.json",
                //        Name = desc.GroupName.ToUpperInvariant()
                //    });

                //foreach (var endpoint in swaggerEndpoints)
                //{
                //    options.SwaggerEndpoint(endpoint.Url, endpoint.Name);
                //}
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "My App API");
                options.SwaggerEndpoint("/swagger/v3/swagger.json", "Elsa Workflows API");

            });
        }
        return app;
    }
}
