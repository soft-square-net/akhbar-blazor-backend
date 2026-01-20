using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace FSH.Starter.BlazorShared.Layout.Components.Menu;
public class MenuReference : IMenuReference
{
    public Guid Id => throw new NotImplementedException();

    public RenderFragment? RenderFragment { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public Task<MenuResult?> Result => throw new NotImplementedException();

    public TaskCompletionSource<bool> RenderCompleteTaskCompletionSource => throw new NotImplementedException();

    public object? Menu => throw new NotImplementedException();

    public void Close()
    {
        throw new NotImplementedException();
    }

    public void Close(MenuResult? result)
    {
        throw new NotImplementedException();
    }

    public bool Dismiss(MenuResult? result)
    {
        throw new NotImplementedException();
    }

    public Task<T?> GetReturnValueAsync<[DynamicallyAccessedMembers((DynamicallyAccessedMemberTypes)(-1))] T>()
    {
        throw new NotImplementedException();
    }

    public void InjectMenu(object inst)
    {
        throw new NotImplementedException();
    }

    public void InjectRenderFragment(RenderFragment rf)
    {
        throw new NotImplementedException();
    }
}
