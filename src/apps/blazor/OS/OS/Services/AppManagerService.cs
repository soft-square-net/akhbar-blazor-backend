using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSH.Starter.Blazor.OS.Interfaces;
using FSH.Starter.Blazor.OS.Services.Interfaces;

namespace FSH.Starter.Blazor.OS.Services;
public class AppManagerService : IAppManagerService
{
    private readonly IEnumerable<IAppType> _installedApps;
    public AppManagerService()
    {
        _installedApps = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IAppType).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            .Select(type => (IAppType)Activator.CreateInstance(type)!);
    }
    public AppManagerService(IEnumerable<IAppType> apps)
    {
        _installedApps = apps;
    }
    public IEnumerable<IAppType> InstalledApps => _installedApps;
}
