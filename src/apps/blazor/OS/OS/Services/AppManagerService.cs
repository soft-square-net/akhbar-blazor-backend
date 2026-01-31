
using FSH.Starter.Blazor.OS.Abstractions.Interfaces;
using FSH.Starter.Blazor.OS.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FSH.Starter.Blazor.OS.Services;
public class AppManagerService<TDialog, TOptions, TResult> : IAppManagerService<TDialog, TOptions, TResult>
{
    private readonly IEnumerable<IAppType<ComponentBase>> _installedApps;
    public AppManagerService()
    {
        _installedApps = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IAppType<ComponentBase>).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            .Select(type => (IAppType<ComponentBase>)Activator.CreateInstance(type)!);
    }
    public AppManagerService(IEnumerable<IAppType<ComponentBase>> apps)
    {
        _installedApps = apps;
    }
    public IEnumerable<IAppType<ComponentBase>> InstalledApps => _installedApps;

    public Action<IAppType<ComponentBase>> OnAppLaunched { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Action<IAppType<ComponentBase>> OnAppClosed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public IEnumerable<IAppInstance<ComponentBase>> ActiveApps => throw new NotImplementedException();

    public void CloseApp(IAppInstance<ComponentBase> app)
    {
        throw new NotImplementedException();
    }

    public void LaunchApp(Type appType, string targetPath = "")
    {
        throw new NotImplementedException();
    }

    public void LaunchApp(Type appType, string zctionName, IDictionary<string, object> requestParams, IOsWindow<TDialog, TOptions, TResult> openerWindow)
    {
        throw new NotImplementedException();
    }
}
