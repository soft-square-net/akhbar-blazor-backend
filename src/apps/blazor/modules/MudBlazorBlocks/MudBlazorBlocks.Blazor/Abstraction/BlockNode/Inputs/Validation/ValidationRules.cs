// ValidationRules.cs
using System.Text.Json.Serialization;
namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction.BlockNode;

public class DependentRule
{
    // The ID of the field this field depends on
    public string TargetFieldId { get; set; } = string.Empty;

    // Condition types: Equals, NotEquals, GreaterThan, LessThan, HasValue
    public string Condition { get; set; } = "Equals";

    // Expected value to satisfy the condition
    public object? ExpectedValue { get; set; }

    // What to enforce when the condition passes (e.g., "Required", "GreaterThanTarget")
    public string Enforcement { get; set; } = "Required";

    // Custom error message
    public string Message { get; set; } = "Validation failed based on dependent field.";
}

public class AsyncValidationRule
{
    // Identifier for the remote action (e.g., "CheckUsernameAvailability")
    public string RuleName { get; set; } = string.Empty;
    
    // API endpoint path or service key
    public string EndpointUrl { get; set; } = string.Empty;

    // Error message if validation fails
    public string Message { get; set; } = "Validation check failed.";
}

public class ValidationRules
{
    public bool IsRequired { get; set; }
    public string? RequiredMessage { get; set; }

    public double? Min { get; set; }
    public double? Max { get; set; }
    public string? RangeMessage { get; set; }

    public int? MinLength { get; set; }
    public int? MaxLength { get; set; }

    public string? RegexPattern { get; set; }
    public string? RegexMessage { get; set; }
    
    
    // List of cross-field rules
    public List<DependentRule>? DependentRules { get; set; }
    
    // Async validation configuration
    public AsyncValidationRule? AsyncRule { get; set; }
}

