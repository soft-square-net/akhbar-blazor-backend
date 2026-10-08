using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction;

public enum FilterOperator
{
    Equals,
    NotEquals,
    Contains,
    StartsWith,
    EndsWith,
    GreaterThan,
    GreaterThanOrEqual,
    LessThan,
    LessThanOrEqual
}

/// <summary>
/// Represents an atomic filter condition (e.g., PropertyName "Price" >= 100).
/// </summary>
public class FilterRule
{
    public string PropertyName { get; set; } = string.Empty; // e.g., "Name" or "Price"
    public string Operator { get; set; } = "Contains";      // "Equals", "Contains", ">=", etc.
    public object? Value { get; set; }
}

public enum LogicalGroupOperator
{
    And,
    Or
}

/// <summary>
/// Composite structure capable of representing single rules or nested groups of rules.
/// </summary>
public class FilterGroup
{
    /// <summary>
    /// Specifies how children/rules in this group are joined (AND / OR).
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public LogicalGroupOperator GroupOperator { get; set; } = LogicalGroupOperator.And;

    /// <summary>
    /// Atomic rules inside this group.
    /// </summary>
    public List<FilterRule> Rules { get; set; } = new();

    /// <summary>
    /// Nested child groups inside this group.
    /// </summary>
    public List<FilterGroup> Groups { get; set; } = new();
}

public static class TreeNodeFilterBuilder<T> where T : class, new()
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    private static readonly MethodInfo StringContainsMethod = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })!;
    private static readonly MethodInfo StringStartsWithMethod = typeof(string).GetMethod(nameof(string.StartsWith), new[] { typeof(string) })!;
    private static readonly MethodInfo StringEndsWithMethod = typeof(string).GetMethod(nameof(string.EndsWith), new[] { typeof(string) })!;

    #region Single Rule Building

    /// <summary>
    /// Builds an Expression filter for TreeNode<T> from property name, string operator, and target value.
    /// </summary>
    public static Expression<Func<TreeNode<T>, bool>> BuildFilter(string propertyName, string op, object? value)
    {
        var operatorEnum = ParseOperator(op);
        return BuildFilter(propertyName, operatorEnum, value);
    }

    /// <summary>
    /// Builds an Expression filter for TreeNode<T> from a single rule.
    /// </summary>
    public static Expression<Func<TreeNode<T>, bool>> BuildFilter(string propertyName, FilterOperator op, object? value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);

        var parameter = Expression.Parameter(typeof(TreeNode<T>), "node");
        Expression propertyAccess = ResolvePropertyPath(parameter, propertyName);
        Expression targetValue = FormatValueExpression(propertyAccess.Type, value);
        Expression comparison = BuildComparison(propertyAccess, op, targetValue);

        return Expression.Lambda<Func<TreeNode<T>, bool>>(comparison, parameter);
    }

    #endregion

    #region JSON Filter Parsing

    /// <summary>
    /// Parses a JSON payload (single FilterRule, list of FilterRules, or nested FilterGroup) 
    /// and compiles it into a single Expression filter.
    /// </summary>
    public static Expression<Func<TreeNode<T>, bool>> ParseJsonFilter(string json, bool combineWithAnd = true)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(json);

        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        // 1. Handle composite FilterGroup JSON payload
        if (root.ValueKind == JsonValueKind.Object && (root.TryGetProperty("Rules", out _) || root.TryGetProperty("Groups", out _)))
        {
            var group = root.Deserialize<FilterGroup>(JsonOptions);
            return group != null ? BuildGroupFilter(group) : node => true;
        }

        // 2. Handle flat array of FilterRules JSON payload
        var rules = new List<FilterRule>();
        if (root.ValueKind == JsonValueKind.Array)
        {
            foreach (var element in root.EnumerateArray())
            {
                var rule = element.Deserialize<FilterRule>(JsonOptions);
                if (rule != null) rules.Add(rule);
            }
        }
        // 3. Handle single FilterRule JSON payload
        else if (root.ValueKind == JsonValueKind.Object)
        {
            var rule = root.Deserialize<FilterRule>(JsonOptions);
            if (rule != null) rules.Add(rule);
        }

        if (rules.Count == 0)
        {
            return node => true; // Neutral fallback filter
        }

        Expression<Func<TreeNode<T>, bool>> combined = BuildFilter(rules[0].PropertyName, rules[0].Operator, rules[0].Value);

        for (int i = 1; i < rules.Count; i++)
        {
            var nextFilter = BuildFilter(rules[i].PropertyName, rules[i].Operator, rules[i].Value);
            combined = combineWithAnd ? CombineAnd(combined, nextFilter) : CombineOr(combined, nextFilter);
        }

        return combined;
    }

    #endregion

    #region Complex Group Filter Building

    /// <summary>
    /// Converts a composite FilterGroup hierarchy into a single Expression filter.
    /// </summary>
    public static Expression<Func<TreeNode<T>, bool>> BuildGroupFilter(FilterGroup group)
    {
        ArgumentNullException.ThrowIfNull(group);

        var parameter = Expression.Parameter(typeof(TreeNode<T>), "node");
        var body = BuildGroupBody(group, parameter);

        if (body == null)
        {
            return node => true;
        }

        return Expression.Lambda<Func<TreeNode<T>, bool>>(body, parameter);
    }

    private static Expression? BuildGroupBody(FilterGroup group, ParameterExpression parameter)
    {
        var expressions = new List<Expression>();

        foreach (var rule in group.Rules)
        {
            var op = ParseOperator(rule.Operator);
            Expression propertyAccess = ResolvePropertyPath(parameter, rule.PropertyName);
            Expression targetValue = FormatValueExpression(propertyAccess.Type, rule.Value);
            Expression comparison = BuildComparison(propertyAccess, op, targetValue);
            expressions.Add(comparison);
        }

        foreach (var childGroup in group.Groups)
        {
            var childBody = BuildGroupBody(childGroup, parameter);
            if (childBody != null)
            {
                expressions.Add(childBody);
            }
        }

        if (expressions.Count == 0) return null;

        Expression current = expressions[0];
        for (int i = 1; i < expressions.Count; i++)
        {
            current = group.GroupOperator == LogicalGroupOperator.And
                ? Expression.AndAlso(current, expressions[i])
                : Expression.OrElse(current, expressions[i]);
        }

        return current;
    }

    #endregion

    #region Helper Methods

    private static Expression ResolvePropertyPath(Expression param, string propertyPath)
    {
        if (!propertyPath.StartsWith("Value.", StringComparison.OrdinalIgnoreCase) &&
            !propertyPath.Equals("Value", StringComparison.OrdinalIgnoreCase))
        {
            propertyPath = $"Value.{propertyPath}";
        }

        Expression current = param;
        foreach (var member in propertyPath.Split('.'))
        {
            var propInfo = current.Type.GetProperty(
                member,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (propInfo == null)
            {
                throw new ArgumentException($"Property '{member}' not found on type '{current.Type.Name}'.");
            }

            current = Expression.Property(current, propInfo);
        }

        return current;
    }

    private static Expression BuildComparison(Expression left, FilterOperator op, Expression right)
    {
        if (left.Type == typeof(string))
        {
            var nullCheck = Expression.NotEqual(left, Expression.Constant(null, typeof(string)));

            MethodInfo? method = op switch
            {
                FilterOperator.Contains => StringContainsMethod,
                FilterOperator.StartsWith => StringStartsWithMethod,
                FilterOperator.EndsWith => StringEndsWithMethod,
                _ => null
            };

            if (method != null)
            {
                var body = Expression.Call(left, method, right);
                return Expression.AndAlso(nullCheck, body);
            }
        }

        return op switch
        {
            FilterOperator.Equals => Expression.Equal(left, right),
            FilterOperator.NotEquals => Expression.NotEqual(left, right),
            FilterOperator.GreaterThan => Expression.GreaterThan(left, right),
            FilterOperator.GreaterThanOrEqual => Expression.GreaterThanOrEqual(left, right),
            FilterOperator.LessThan => Expression.LessThan(left, right),
            FilterOperator.LessThanOrEqual => Expression.LessThanOrEqual(left, right),
            _ => Expression.Equal(left, right)
        };
    }

    private static Expression FormatValueExpression(Type targetType, object? value)
    {
        if (value == null)
        {
            return Expression.Constant(null, targetType);
        }

        if (value is JsonElement element)
        {
            value = element.ValueKind switch
            {
                JsonValueKind.String => element.GetString(),
                JsonValueKind.Number => element.TryGetInt64(out long l) ? l : element.GetDouble(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                _ => element.GetRawText()
            };
        }

        var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

        object? convertedValue;
        if (underlyingType.IsEnum && value != null)
        {
            convertedValue = Enum.Parse(underlyingType, value.ToString()!, ignoreCase: true);
        }
        else
        {
            convertedValue = Convert.ChangeType(value, underlyingType);
        }

        return Expression.Constant(convertedValue, targetType);
    }

    private static FilterOperator ParseOperator(string op) => op.ToLowerInvariant() switch
    {
        "eq" or "equals" or "==" or "=" => FilterOperator.Equals,
        "neq" or "notequals" or "!=" => FilterOperator.NotEquals,
        "contains" or "like" => FilterOperator.Contains,
        "startswith" => FilterOperator.StartsWith,
        "endswith" => FilterOperator.EndsWith,
        "gt" or ">" or "greaterthan" => FilterOperator.GreaterThan,
        "gte" or ">=" => FilterOperator.GreaterThanOrEqual,
        "lt" or "<" or "lessthan" => FilterOperator.LessThan,
        "lte" or "<=" => FilterOperator.LessThanOrEqual,
        _ => FilterOperator.Contains
    };

    private static Expression<Func<TType, bool>> CombineAnd<TType>(
        Expression<Func<TType, bool>> left,
        Expression<Func<TType, bool>> right)
    {
        var parameter = Expression.Parameter(typeof(TType));
        var leftVisitor = new ParameterVisitor(left.Parameters[0], parameter);
        var rightVisitor = new ParameterVisitor(right.Parameters[0], parameter);

        var body = Expression.AndAlso(
            leftVisitor.Visit(left.Body),
            rightVisitor.Visit(right.Body)
        );

        return Expression.Lambda<Func<TType, bool>>(body, parameter);
    }

    private static Expression<Func<TType, bool>> CombineOr<TType>(
        Expression<Func<TType, bool>> left,
        Expression<Func<TType, bool>> right)
    {
        var parameter = Expression.Parameter(typeof(TType));
        var leftVisitor = new ParameterVisitor(left.Parameters[0], parameter);
        var rightVisitor = new ParameterVisitor(right.Parameters[0], parameter);

        var body = Expression.OrElse(
            leftVisitor.Visit(left.Body),
            rightVisitor.Visit(right.Body)
        );

        return Expression.Lambda<Func<TType, bool>>(body, parameter);
    }

    private class ParameterVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression _oldParameter;
        private readonly ParameterExpression _newParameter;

        public ParameterVisitor(ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            _oldParameter = oldParameter;
            _newParameter = newParameter;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == _oldParameter ? _newParameter : base.VisitParameter(node);
        }
    }

    #endregion


    #region JSON Export Serialization

    /// <summary>
    /// Serializes a TreeNode<T> hierarchy into a JSON string.
    /// </summary>
    public static string ExportToJson(TreeNode<T> node)
    {
        ArgumentNullException.ThrowIfNull(node);
        return JsonSerializer.Serialize(node, JsonOptions);
    }

    /// <summary>
    /// Serializes a collection of TreeNode<T> items into a JSON string.
    /// </summary>
    public static string ExportToJson(IEnumerable<TreeNode<T>> nodes)
    {
        ArgumentNullException.ThrowIfNull(nodes);
        return JsonSerializer.Serialize(nodes, JsonOptions);
    }

    /// <summary>
    /// Serializes a FilterGroup rule hierarchy into a JSON filter structure string.
    /// </summary>
    public static string ExportToJson(FilterGroup group)
    {
        ArgumentNullException.ThrowIfNull(group);
        return JsonSerializer.Serialize(group, JsonOptions);
    }

    /// <summary>
    /// Serializes a single FilterRule or collection of rules into a JSON string.
    /// </summary>
    public static string ExportToJson(IEnumerable<FilterRule> rules)
    {
        ArgumentNullException.ThrowIfNull(rules);
        return JsonSerializer.Serialize(rules, JsonOptions);
    }

    #endregion

    #region JSON Import Deserialization

    /// <summary>
    /// Deserializes a JSON string into a single TreeNode<T> hierarchy.
    /// </summary>
    public static Expression<Func<TreeNode<T>, bool>>? ImportFromJson(string json)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(json);

        return JsonSerializer.Deserialize<Expression<Func<TreeNode<T>, bool>>>(json, JsonOptions);
    }

    /// <summary>
    /// Deserializes a JSON string into a collection of TreeNode<T> items.
    /// </summary>
    public static List<Expression<Func<TreeNode<T>, bool>>> ImportFromJsonList(string json)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(json);

        return JsonSerializer.Deserialize<List<Expression<Func<TreeNode<T>, bool>>>>(json, JsonOptions) ?? new List<Expression<Func<TreeNode<T>, bool>>>();
    }

    /// <summary>
    /// Tries to deserialize a JSON string into a single TreeNode<T>, returning false if parsing fails.
    /// </summary>
    public static bool TryImportFromJson(string json, out Expression<Func<TreeNode<T>, bool>>? result)
    {
        try
        {
            result = ImportFromJson(json);
            return result != null;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    #endregion
}
