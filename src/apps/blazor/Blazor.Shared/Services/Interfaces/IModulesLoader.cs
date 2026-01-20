
using FSH.Starter.BlazorShared.interfaces;

namespace FSH.Starter.BlazorShared.Services.Interfaces;
public interface IModulesLoader
{
    // A list to hold the types of components to render
    public List<Type> ComponentsToRender { get; }

    public event Action OnChange;

    public void AddComponent<T>() where T : IModuleMenu;
    public void AddComponent(IModuleMenu menu);

    public void ClearComponents();

    private void NotifyStateChanged() { }

}
