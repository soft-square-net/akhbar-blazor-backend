
using FSH.Starter.Blazor.OS.Abstractions.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace FSH.Starter.Blazor.OS.Messages;
public class OpenWindowCommand: IRequest<OpenWindowResponse>
{
    public IAppType<ComponentBase> App { get; set; }
    public string TargetPath { get; set; }
    public IDictionary<string,object> Params { get; set; }
}
