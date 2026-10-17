
using Microsoft.Extensions.DependencyInjection;

namespace BlazorModularEngine.Core.Services;

public class DynamicServiceRegistry
{
    private readonly Dictionary<Type, Func<IServiceProvider, object>> _factories = new();
    private readonly Dictionary<Type, object> _singletons = new();

    public void RegisterTransient<TInterface, TImplementation>() where TImplementation : class, TInterface
    {
        _factories[typeof(TInterface)] = sp => ActivatorUtilities.CreateInstance<TImplementation>(sp);
    }

    public void RegisterSingleton<TInterface>(TInterface instance) where TInterface : class
    {
        _singletons[typeof(TInterface)] = instance;
    }

    public void RegisterSingleton(Type serviceType, object instance)
    {
        _singletons[serviceType] = instance;
    }

    public void Register(Type serviceType, Type implementationType, ServiceLifetime lifetime)
    {
        _factories[serviceType] = sp => ActivatorUtilities.CreateInstance(sp, implementationType);
    }

    public T GetService<T>(IServiceProvider sp) where T : class
    {
        return (T)GetService(typeof(T), sp);
    }

    public object GetService(Type type, IServiceProvider sp)
    {
        if (_singletons.TryGetValue(type, out var singleton)) return singleton;
        if (_factories.TryGetValue(type, out var factory)) return factory(sp);
        return null;
    }
}
