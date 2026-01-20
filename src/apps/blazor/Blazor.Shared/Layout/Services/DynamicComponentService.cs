
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using FSH.Starter.BlazorShared.Layout.Services.Interfaces;

namespace FSH.Starter.BlazorShared.Layout.Services;
public class DynamicComponentService : IDynamicComponentService
{
    public event Action OnChange;
    public Type CurrentComponentType { get; private set; }
    public Dictionary<string, object> CurrentParameters { get; private set; }

    public void ShowComponent<T>(Dictionary<string, object> parameters = null) where T : IComponent
    {
        CurrentComponentType = typeof(T);
        CurrentParameters = parameters;
        NotifyStateChanged();
    }

    public void HideComponent()
    {
        CurrentComponentType = null;
        CurrentParameters = null;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
