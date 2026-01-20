//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using FSH.Starter.Blazor.Modules.Services.Interfaces;
//using FSH.Starter.BlazorShared.interfaces;
//using FSH.Starter.Shared.Authorization;

//namespace FSH.Starter.Blazor.Modules.Services;
//public class ModulesLoader : IModulesLoader
//{
//    public List<Type> ComponentsToRender { get; private set; } = new();

//    // A list to hold the types of components to render
//    // public List<Type> ComponentsToRender { get; private set; } = new List<Type>();

//    public event Action OnChange;

//    public void AddComponent<T>() where T : IModuleMenu
//    {
//        ComponentsToRender.Add(typeof(T));
//        NotifyStateChanged();
//    }

//    public void AddComponent(IModuleMenu menu)
//    {
//        if(menu is not null)
//        {
//            ComponentsToRender.Add(menu.GetType());
//            NotifyStateChanged();
//        }
//    }

//    public void ClearComponents()
//    {
//        ComponentsToRender.Clear();
//        NotifyStateChanged();
//    }

//    private void NotifyStateChanged() => OnChange?.Invoke();
//}
