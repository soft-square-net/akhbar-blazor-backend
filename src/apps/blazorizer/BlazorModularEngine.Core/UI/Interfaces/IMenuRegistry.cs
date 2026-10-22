
using BlazorModularEngine.Core.Models;

namespace BlazorModularEngine.Core.UI.Interfaces;

public interface IMenuRegistry
{
    IEnumerable<MenuItemModel> GetItemsForRegion(string region);
}
