using System.Text.Json.Serialization;
using FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction.BlockNode.Inputs.Visibility;

namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction.BlockNode.Inputs;

public class BlockNodeInput
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; }
    public string Label { get; set; } = string.Empty;
    // public BlockNodeInputType Type { get; set; }
    public string Type { get; set; } = "text"; // text, number, checkbox, select, textarea

    public object? Value { get; set; }
    
    [JsonIgnore]
    public bool IsValidating { get; set; } // Tracks field-level async validation state
    // Added Validation Configuration
    public ValidationRules? Validation { get; set; }
    
    
    // Conditional visibility configuration
    public VisibilityRule? VisibilityRule { get; set; }
    
    // Runtime state tracking
    [JsonIgnore]
    public List<string> ValidationErrors { get; set; } = new();

    [JsonIgnore]
    public bool IsValid => ValidationErrors.Count == 0;
    
    public List<string>? Options { get; set; } // Used when Type == "select"
    
    public BlockNodeInput Clone()
    {
        return new BlockNodeInput
        {
            Id = this.Id,
            Label = this.Label,
            Type = this.Type,
            Value = this.Value,
            Options = this.Options != null ? new List<string>(this.Options) : null,
            Validation = this.Validation,
            VisibilityRule = this.VisibilityRule,
            ValidationErrors = new List<string>()
        };
    }
}

