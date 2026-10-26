using System.Text.Json.Serialization;
using FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction.BlockNode.Inputs.Visibility;

namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction.BlockNode.Inputs;

public class BlockNodeInput
{
    public string Name { get; set; }

    public object? Value { get; set; }
    
    // Defines which wizard step this field belongs to (1-based index)
    public int Step { get; set; } = 1;
    
    // Optional raw JSON Schema string or object for IntelliSense/validation
    public string? JsonSchema { get; set; }
    
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
    



    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Label { get; set; } = "New Field";
    public string FieldKey { get; set; } = "field_key";
    public FieldType Type { get; set; } = FieldType.Text;
    public string DefaultValue { get; set; } = string.Empty;
    public string Placeholder { get; set; } = string.Empty;

    // Validation Rules
    public bool IsRequired { get; set; }
    public int? MinLength { get; set; }
    public int? MaxLength { get; set; }
    public string? RegexPattern { get; set; }
    public string ErrorMessage { get; set; } = "Invalid field value";

    // Options for Select / Radio fields
    public List<string> Options { get; set; } = new();

    // Conditional Visibility Rule
    public string? VisibleWhenFieldKey { get; set; }
    public string? VisibleEqualsValue { get; set; }

    public BlockNodeInput Clone()
    {
        return new BlockNodeInput
        {
            Id = Guid.NewGuid().ToString(),
            Label = Label,
            FieldKey = FieldKey,
            Type = Type,
            DefaultValue = DefaultValue,
            Placeholder = Placeholder,
            IsRequired = IsRequired,
            MinLength = MinLength,
            MaxLength = MaxLength,
            RegexPattern = RegexPattern,
            ErrorMessage = ErrorMessage,
            Options = new List<string>(Options),
            VisibleWhenFieldKey = VisibleWhenFieldKey,
            VisibleEqualsValue = VisibleEqualsValue,
            Value = this.Value,
            Validation = this.Validation,
            VisibilityRule = this.VisibilityRule,
            ValidationErrors = new List<string>()
        };

        
    }
}

public enum FieldType
{
    Text,
    Number,
    Checkbox,
    Select,
    Date,
    ContainerSection,
    Textarea,
    Json
}

