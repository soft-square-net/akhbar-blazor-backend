using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorModularEngine.Core.UI;

/// <summary>
/// Registry allowing plugins to inject UI component types into named slots in the host layout (e.g., sidebar, dashboard).
/// </summary>
public class PluginSlotRegistry
{
    private readonly Dictionary<string, List<Type>> _slots = new(StringComparer.OrdinalIgnoreCase);

    public void RegisterComponent(string slotName, Type componentType)
    {
        if (!_slots.ContainsKey(slotName)) _slots[slotName] = new();
        if (!_slots[slotName].Contains(componentType))
        {
            _slots[slotName].Add(componentType);
        }
    }

    public IEnumerable<Type> GetComponentsForSlot(string slotName)
    {
        return _slots.TryGetValue(slotName, out var list) ? list : Array.Empty<Type>();
    }
}
