using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorModularEngine.Core.Hooks;

/// <summary>
/// Manages WordPress-style Actions (events) and Filters (data interceptors) 
/// allowing decoupled communication between plugins and the host app.
/// </summary>
public class PluginHookManager
{
    private readonly Dictionary<string, List<Action<object[]>>> _actions = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, List<Func<object, object, object>>> _filters = new(StringComparer.OrdinalIgnoreCase);

    public void AddAction(string actionName, Action<object[]> callback)
    {
        if (!_actions.ContainsKey(actionName)) _actions[actionName] = new();
        _actions[actionName].Add(callback);
    }

    public void DoAction(string actionName, params object[] args)
    {
        if (_actions.TryGetValue(actionName, out var callbacks))
        {
            foreach (var callback in callbacks)
            {
                try { callback(args); } catch (Exception ex) { Console.WriteLine($"Error in action '{actionName}': {ex.Message}"); }
            }
        }
    }

    public void AddFilter(string filterName, Func<object, object, object> filterFunc)
    {
        if (!_filters.ContainsKey(filterName)) _filters[filterName] = new();
        _filters[filterName].Add(filterFunc);
    }

    public T ApplyFilters<T>(string filterName, T value, object context = null)
    {
        if (!_filters.TryGetValue(filterName, out var filters)) return value;

        object current = value;
        foreach (var filter in filters)
        {
            try { current = filter(current, context); } catch (Exception ex) { Console.WriteLine($"Error in filter '{filterName}': {ex.Message}"); }
        }
        return (T)current;
    }
}
