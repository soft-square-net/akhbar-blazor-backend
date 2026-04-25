using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FSH.Framework.Infrastructure.OpenApi;

public class ElsaOpenApiFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {

        // Modify the operation based on the route
        if (context.ApiDescription.RelativePath.StartsWith("elsa/api/", StringComparison.OrdinalIgnoreCase))
        {
            context.ApiDescription.GroupName = "elsa";
        }
    }
}
