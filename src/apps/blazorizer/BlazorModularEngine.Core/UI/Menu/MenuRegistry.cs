
using BlazorModularEngine.Core.Common.Interfaces;
using BlazorModularEngine.Core.Models;
using BlazorModularEngine.Core.UI.Interfaces;

namespace BlazorModularEngine.Core.UI.Menu;

public class MenuRegistry : IMenuRegistry
{
    private readonly List<MenuItemModel> _allItems = new();

    public MenuRegistry(IEnumerable<IBlazorModule> modules)
    {
        // Automatically aggregate menu items from all registered modules
        foreach (var module in modules)
        {
            var items = module.GetMenuItems();
            if (items != null)
            {
                _allItems.AddRange(items);
            }
        }
    }

    public IEnumerable<MenuItemModel> GetItemsForRegion(string region)
    {
        return _allItems
            .Where(i => i.Region.Equals(region, StringComparison.OrdinalIgnoreCase))
            .OrderBy(i => i.Order);
    }
}
