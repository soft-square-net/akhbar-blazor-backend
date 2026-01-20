//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using FSH.Starter.BlazorShared.interfaces;
//using FSH.Starter.Shared.Authorization;

//namespace FSH.Starter.Blazor.Modules.Services.Interfaces;
//public interface IModulesLoader
//{
//    // A list to hold the types of components to render
//    public List<Type> ComponentsToRender { get; }

//    public event Action OnChange;

//    public void AddComponent<T>() where T : IModuleMenu;
//    public void AddComponent(IModuleMenu menu);

//    public void ClearComponents();

//    private void NotifyStateChanged() { }

//}
