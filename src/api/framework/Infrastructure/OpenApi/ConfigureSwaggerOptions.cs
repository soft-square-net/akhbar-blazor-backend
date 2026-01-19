using System.Text;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FSH.Framework.Infrastructure.OpenApi;
public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
    /// </summary>
    /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IConfiguration configuration) { 
        this.provider = provider; 
    }

    /// <inheritdoc />
    public void Configure(SwaggerGenOptions options)
    {
        // add a swagger document for each discovered API version
        // note: you might choose to skip or document deprecated API versions differently
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }

        options.SwaggerDoc("identity", new OpenApiInfo
        {
            Title = "Identity Module API",
            Version = "v1"
        });
        options.SwaggerDoc("tenants", new OpenApiInfo
        {
            Title = "Tenants Module API",
            Version = "v1"
        });
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var text = new StringBuilder(".NET 8 Starter Kit with Vertical Slice Architecture!");
        var info = new OpenApiInfo()
        {
            Title = $"{description.GroupName} Module in FSH.Starter.WebApi",
            Version = description.ApiVersion.ToString(),
            Contact = new OpenApiContact() { Name = "Mukesh Murugan & Ahmed Galal", Email = "hello@codewithmukesh.com" }
        };

        if (description.IsDeprecated)
        {
            text.Append(" This API version has been deprecated.");
        }

        info.Description = text.ToString();

        return info;
    }
}
