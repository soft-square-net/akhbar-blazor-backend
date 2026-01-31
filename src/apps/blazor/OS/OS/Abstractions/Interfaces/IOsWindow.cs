using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSH.Starter.Blazor.OS.Types;
using Microsoft.AspNetCore.Components;

namespace FSH.Starter.Blazor.OS.Abstractions.Interfaces;
public interface IOsWindow<TDialog,TOptions,TResult>
{
    TDialog Dialog { get; set; }
    TOptions Options { get; set; }
    TResult Result { get; set; }
    public IAppInstance<ComponentBase> App { get; set; }
    IOsWindow<TDialog, TOptions, TResult> OpenerWindow { get; set; }
    public Guid Id { get; set; }
    public string Title { get; set; }
    public Rectangle Rect { get; set; }
    public IDictionary<string, object> WinParams { get; }
    public bool IsActive { get; }
    public bool IsMinimized { get; set; }
    public bool IsMaximized { get; set; }
    public int ZIndex { get; set; }
}
