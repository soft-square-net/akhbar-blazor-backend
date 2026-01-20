using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace FSH.Starter.BlazorShared.Layout.Services.Interfaces;
public interface IDynamicComponentService
{
    event Action OnChange;
    Type CurrentComponentType { get; }
    Dictionary<string, object> CurrentParameters { get; }
    void ShowComponent<T>(Dictionary<string, object> parameters = null) where T : IComponent;
    void HideComponent();
}
