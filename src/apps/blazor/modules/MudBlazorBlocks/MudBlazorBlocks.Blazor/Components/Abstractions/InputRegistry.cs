using FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction.BlockNode;
using FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction.BlockNode.Inputs;
using FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Components.Blocks.Inputs;
using FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Components.Blocks.Inputs.Base;
using FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Components.Blocks.Inputs.Custome;

namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Components.Abstractions;

using System;
using System.Collections.Generic;

public static class InputRegistry
{
    private static readonly Dictionary<string, Type> Registry = new(StringComparer.OrdinalIgnoreCase)
    {
        { "text", typeof(TextInput) },
        { "number", typeof(NumberInput) },
        { "checkbox", typeof(CheckboxInput) },
        { "select", typeof(SelectInput) },
        { "textarea", typeof(TextAreaInput) },
        { "json", typeof(JsonEditorInput) },   // Custom complex field mapping
        { "code", typeof(JsonEditorInput) }
    };

    public static Type GetInputComponent(string inputType)
    {
        return Registry.TryGetValue(inputType, out var componentType) 
            ? componentType 
            : typeof(TextInput);
    }

    public static void RegisterComponent(string typeName, Type componentType)
    {
        if (!typeof(InputComponentBase).IsAssignableFrom(componentType))
        {
            throw new ArgumentException(
                $"Component type {componentType.Name} must derive from {nameof(InputComponentBase)}.", 
                nameof(componentType));
        }

        Registry[typeName] = componentType;
    }
}
