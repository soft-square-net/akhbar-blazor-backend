
using FSH.Starter.Blazor.OS.Abstractions.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace FSH.Starter.Blazor.OS.Messages;
public class OpenNestedWindow<TDialog, TOptions, TResult> : IRequest<OpenNestedWindowResponse>
{
    public IAppType<ComponentBase> app { get; set; }
    public IOsWindow<TDialog, TOptions, TResult> OpenerWindow { get; set; }
    public string TargetPath { get; set; }
    public IDictionary<string,object> Params { get; set; }
}
