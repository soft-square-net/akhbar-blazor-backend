
using FSH.Starter.Blazor.OS.Abstractions.Interfaces;
using MudBlazor;

namespace FSH.Starter.Blazor.Modules.DesktopLayout.Blazor.OS;
public record BlazorOsWindowOptions : IOsWindowsOptions<DialogOptions>
{
    public string ActionsClass { get; set; }
    public string ContentClass { get; set; }
    public string ContentStyle { get; set; }
    public bool Gutters { get; set; }
    public string TitleClass { get; set; }
    public string Title { get; set; }
    public bool Dragable { get; set; }
    public bool Visible { get; set; }
    public DialogOptions Options { get; set; }
    public string Class { get; set; }
    public string Style { get; set; }
    public object Tag { get; set; }
    public Dictionary<string, object> UserAttributes { get; set; }
}

