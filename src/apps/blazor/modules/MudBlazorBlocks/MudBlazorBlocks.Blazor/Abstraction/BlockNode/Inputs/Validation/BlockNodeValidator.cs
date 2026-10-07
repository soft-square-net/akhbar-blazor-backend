
// BlockNodeValidator.cs
using System;
using System.Linq;
using System.Text.RegularExpressions;
using FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction.BlockNode.Inputs;
using FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction.BlockNode.Inputs.Visibility;

namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction.BlockNode;

public static class BlockNodeValidator
{
    public static bool ValidateInput(BlockNodeInput input, BlockNode parentNode)
    {
        input.ValidationErrors.Clear();

        // Skip validation completely if field is hidden by visibility rules
        if (!BlockNodeVisibilityEvaluator.IsFieldVisible(input, parentNode))
        {
            return true;
        }

        var rules = input.Validation;
        if (rules == null) return true;

        var strVal = input.Value?.ToString();
        var isNullOrEmpty = string.IsNullOrWhiteSpace(strVal);

        // --- Standard Single-Field Validations ---
        if (rules.IsRequired && isNullOrEmpty)
        {
            input.ValidationErrors.Add(rules.RequiredMessage ?? $"{input.Label} is required.");
        }

        if (!isNullOrEmpty && (rules.Min.HasValue || rules.Max.HasValue))
        {
            if (double.TryParse(strVal, out double numVal))
            {
                if (rules.Min.HasValue && numVal < rules.Min.Value)
                    input.ValidationErrors.Add(rules.RangeMessage ?? $"{input.Label} must be at least {rules.Min.Value}.");

                if (rules.Max.HasValue && numVal > rules.Max.Value)
                    input.ValidationErrors.Add(rules.RangeMessage ?? $"{input.Label} cannot exceed {rules.Max.Value}.");
            }
        }

        // --- Dependent / Cross-Field Validations ---
        if (rules.DependentRules != null && rules.DependentRules.Count > 0)
        {
            foreach (var depRule in rules.DependentRules)
            {
                var targetField = parentNode.Inputs.FirstOrDefault(x => x.Id == depRule.TargetFieldId);
                if (targetField == null) continue;

                bool isConditionMet = EvaluateCondition(targetField.Value, depRule.Condition, depRule.ExpectedValue);

                if (isConditionMet)
                {
                    EnforceDependentRule(input, targetField, depRule, strVal, isNullOrEmpty);
                }
            }
        }

        return input.IsValid;
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
            _ => false
        };
    }

    private static void EnforceDependentRule(
        BlockNodeInput currentInput, 
        BlockNodeInput targetInput, 
        DependentRule rule, 
        string? strVal, 
        bool isNullOrEmpty)
    {
        switch (rule.Enforcement.ToLower())
        {
            case "required":
                if (isNullOrEmpty)
                {
                    currentInput.ValidationErrors.Add(rule.Message);
                }
                break;

            case "greaterthantarget":
                if (!isNullOrEmpty && 
                    double.TryParse(strVal, out double currentNum) && 
                    double.TryParse(targetInput.Value?.ToString(), out double targetNum))
                {
                    if (currentNum <= targetNum)
                    {
                        currentInput.ValidationErrors.Add(rule.Message);
                    }
                }
                break;
        }
    }

    public static bool ValidateNode(BlockNode node)
    {
        bool isValid = true;
        foreach (var input in node.Inputs)
        {
            if (!ValidateInput(input, node))
            {
                isValid = false;
            }
        }
        return isValid;
    }
}
