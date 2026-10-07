
// VisibilityRules.cs

namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction.BlockNode.Inputs.Visibility;
public class VisibilityRule
{
    // The ID of the field this field's visibility depends on
    public string TargetFieldId { get; set; } = string.Empty;

    // Condition types: Equals, NotEquals, HasValue, GreaterThan, LessThan, Contains
    public string Condition { get; set; } = "Equals";

    // Expected value to satisfy the condition
    public object? ExpectedValue { get; set; }
}
