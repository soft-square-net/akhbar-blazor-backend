using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorModularEngine.Core.Services;

public class RuntimeServiceCollectionAdapter : List<ServiceDescriptor>, IServiceCollection
{
    private readonly IServiceCollection _fallbackCollection;
    private readonly DynamicServiceRegistry _dynamicRegistry;
    private readonly bool _isAppRunning;

    public RuntimeServiceCollectionAdapter(IServiceCollection fallbackCollection, DynamicServiceRegistry dynamicRegistry, bool isAppRunning)
    {
        _fallbackCollection = fallbackCollection;
        _dynamicRegistry = dynamicRegistry;
        _isAppRunning = isAppRunning;
    }

    public new void Add(ServiceDescriptor item)
    {
        if (_isAppRunning)
        {
            if (item.ImplementationType != null)
            {
                _dynamicRegistry.Register(item.ServiceType, item.ImplementationType, item.Lifetime);
            }
            else if (item.ImplementationInstance != null)
            {
                _dynamicRegistry.RegisterSingleton(item.ServiceType, item.ImplementationInstance);
            }
        }
        else
        {
            _fallbackCollection.Add(item);
        }
        base.Add(item);
    }
}
