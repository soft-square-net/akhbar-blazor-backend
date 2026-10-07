

// InputComponentBase.cs
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction.BlockNode.Inputs;
public abstract class InputComponentBase : ComponentBase
{
    [Parameter, EditorRequired]
    public BlockNodeInput Input { get; set; } = default!;

    [Parameter]
    public EventCallback<BlockNodeInput> InputChanged { get; set; }

    [CascadingParameter]
    public EditContext? EditContext { get; set; }

    protected FieldIdentifier FieldIdentifier => 
        new FieldIdentifier(Input, nameof(BlockNodeInput.Value));

    protected string StringValue => Input.Value?.ToString() ?? string.Empty;
    protected double NumberValue => double.TryParse(Input.Value?.ToString(), out var result) ? result : 0;
    protected bool BoolValue => bool.TryParse(Input.Value?.ToString(), out var result) && result;

    // Use EditContext's built-in field CSS class provider
    protected string FieldCssClass => EditContext?.FieldCssClass(FieldIdentifier) ?? "form-control";

    protected async Task UpdateValueAsync(object? newValue)
    {
        Input.Value = newValue;

        // Notify EditContext that the field changed
        if (EditContext != null)
        {
            EditContext.NotifyFieldChanged(FieldIdentifier);
        }

        await InputChanged.InvokeAsync(Input);
    }

    protected Task OnTextChanged(ChangeEventArgs e) => UpdateValueAsync(e.Value?.ToString());
    protected Task OnNumberChanged(ChangeEventArgs e) => UpdateValueAsync(double.TryParse(e.Value?.ToString(), out var numVal) ? numVal : null);
    protected Task OnBoolChanged(ChangeEventArgs e) => UpdateValueAsync(e.Value is bool b ? b : false);
}
