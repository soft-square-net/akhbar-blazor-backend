using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.Starter.Blazor.Shared.Routing;
public class AppRoute : AppRouteBase<string>
{
    public override string Params    
    {
        get
        {
            StringBuilder result = new();
            foreach (var param in ActionParameters) {
                result.Append($"{param.Key}/{param.Value}/");
            }
            return result.ToString().TrimEnd('/');
        }
    }
    public override string GetRouteUrl()
    {
        return $"{App}/{Endpoint}/{Action}/{Params}".TrimEnd('/');
    }
}
