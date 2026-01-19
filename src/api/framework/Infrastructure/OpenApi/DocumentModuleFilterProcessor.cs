using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FSH.Framework.Infrastructure.OpenApi;
public class DocumentModuleFilterProcessor : IDocumentFilter
{
    private readonly IConfiguration _configuration;

    public DocumentModuleFilterProcessor(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    /// <inheritdoc/>
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        SwashbuckleFiltersConfig SwashbuckleFilters = _configuration.GetRequiredSection("Swashbuckle").Get<SwashbuckleFiltersConfig>();

        if (SwashbuckleFilters.Filters.Enabled)
        {
            Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            if (SwashbuckleFilters.Filters.SchemaAssemblyStartsWith.Any())
            {
                loadedAssemblies = loadedAssemblies.Where(asm => !SwashbuckleFilters.Filters.SchemaAssemblyStartsWith.Any(str => asm.FullName.StartsWith(str))).ToArray();
            }
            if (SwashbuckleFilters.Filters.ExecludeSchemaAssemblyStartsWith.Any())
            {
                loadedAssemblies = loadedAssemblies.Where(asm => SwashbuckleFilters.Filters.ExecludeSchemaAssemblyStartsWith.Any(str => asm.FullName.StartsWith(str))).ToArray();
            }
            List<Type> types = new List<Type>();
            foreach (Assembly assembly in loadedAssemblies)
            {
                types.AddRange(assembly.GetTypes());
            }

            foreach (var type in types)
            {
                context.SchemaRepository.Schemas.Remove(type.Name);
            }

            List<string> keysToRemove = swaggerDoc.Paths.Select(path => path.Key).ToList();
            foreach (var pathPrefix in SwashbuckleFilters.Filters.PathStartsWith)
            {
                keysToRemove = keysToRemove
                    .Where(key => !key.StartsWith(pathPrefix))
                    .ToList();

            }
            foreach (var key in keysToRemove)
            {
                swaggerDoc.Paths.Remove(key);
            }

            //context.ApiDescriptions.ToList().ForEach(apiDesc =>
            //{
            //    if (apiDesc.RelativePath != null && !apiDesc.RelativePath.StartsWith(pathPrefix))
            //    {
            //        var toRemove = context.ApiDescriptions
            //            .FirstOrDefault(ad => ad.RelativePath == apiDesc.RelativePath);
            //        if (toRemove != null)
            //        {
            //            context.ApiDescriptions.Remove(toRemove);
            //        }
            //    }
            //});
            //context.SchemaRepository.Schemas.Remove(id);

        }
    }
}

//public bool Process(OperationProcessorContext context)
//{
//    // Only include if the path starts with "/api/v1/orders"
//    return context.OperationDescription.Path.StartsWith("/api/v1/orders");
//}

//.Where(pathItem => pathItem.Value.Operations
//    .Any(op => op.Value.RequestBody?.Content
//        .Any(c => c.Value.Schema.Reference != null && c.Value.Schema.Reference.Id == id) == true))
