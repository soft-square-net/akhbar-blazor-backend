using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSH.Starter.Blazor.OS.Abstractions.Interfaces;
using FSH.Starter.Blazor.OS.Types;
using Microsoft.AspNetCore.Components;

namespace FSH.Starter.Blazor.OS.Abstractions;
internal abstract class WindowBase<TDialog, TOptions, TResult> : ComponentBase ,IOsWindow<TDialog, TOptions, TResult>
{
    /// <summary>
    ///  Dialog's App Instance 
    ///  Ex: MudDialogInstance for MudBlazor Dialogs
    /// </summary>
    public IAppInstance<ComponentBase> App { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Guid Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string Title { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Rectangle Rect { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public IDictionary<string, object> WinParams => throw new NotImplementedException();

    public bool IsActive => throw new NotImplementedException();

    public bool IsMinimized { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool IsMaximized { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int ZIndex { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public IOsWindow<TDialog, TOptions, TResult> OpenerWindow { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public TDialog Dialog { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public TOptions Options { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public TResult Result { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    IOsWindow<TDialog, TOptions, TResult> IOsWindow<TDialog, TOptions, TResult>.OpenerWindow { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}
