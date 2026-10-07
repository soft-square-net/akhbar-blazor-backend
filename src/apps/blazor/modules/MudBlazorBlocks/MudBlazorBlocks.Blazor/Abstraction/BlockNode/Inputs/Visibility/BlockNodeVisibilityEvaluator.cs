
// BlockNodeVisibilityEvaluator.cs

namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction.BlockNode.Inputs.Visibility;
public static class BlockNodeVisibilityEvaluator
{
    public static bool IsFieldVisible(BlockNodeInput input, BlockNode parentNode)
    {
        var rule = input.VisibilityRule;

        // If no visibility rule is specified, show the field by default
        if (rule == null || string.IsNullOrEmpty(rule.TargetFieldId))
        {
            return true;
        }

        // Find target field in the parent node
        var targetField = parentNode.Inputs.FirstOrDefault(x => x.Id == rule.TargetFieldId);
        if (targetField == null)
        {
            return true;
        }

        // Evaluate target field visibility recursively (if target field itself is hidden, hide dependent field too)
        if (!IsFieldVisible(targetField, parentNode))
        {
            return false;
        }

        return EvaluateCondition(targetField.Value, rule.Condition, rule.ExpectedValue);
    }

    private static bool EvaluateCondition(object? targetValue, string condition, object? expectedValue)
    {
        var targetStr = targetValue?.ToString() ?? string.Empty;
        var expectedStr = expectedValue?.ToString() ?? string.Empty;

        return condition.ToLower() switch
        {
            "equals" => string.Equals(targetStr, expectedStr, StringComparison.OrdinalIgnoreCase),
            "notequals" => !string.Equals(targetStr, expectedStr, StringComparison.OrdinalIgnoreCase),
            "hasvalue" => !string.IsNullOrWhiteSpace(targetStr),
            "greaterthan" => double.TryParse(targetStr, out var t) && double.TryParse(expectedStr, out var e) && t > e,
            "lessthan" => double.TryParse(targetStr, out var tVal) && double.TryParse(expectedStr, out var eVal) && tVal < eVal,
            "contains" => targetStr.IndexOf(expectedStr, StringComparison.OrdinalIgnoreCase) >= 0,
            _ => true
        };
    }
}
