using FSH.Starter.Blazor.Modules.Services;

namespace FSH.Starter.Blazor.Modules.Plugins.Hooks.Base;

public abstract class PluginHookBase
{
    public string Name { get; set; } = string.Empty;
    public HookType HookType { get; set; } = HookType.Custom;
}

// 2. Generic derived class enforcing a strongly-typed return type T
public class PluginHook<T> : PluginHookBase
{
    public Func<PluginManager, Dictionary<string, object?>, T> HookFunc { get; set; }

    public PluginHook(string name, Func<PluginManager, Dictionary<string, object?>, T> hookFunc, HookType hookType = HookType.Custom)
    {
        Name = name;
        HookFunc = hookFunc;
        HookType = hookType;
    }

    // Optional factory method for clean instantiation
    public static PluginHook<T> Create(string name, Func<PluginManager, Dictionary<string, object?>, T> hookFunc, HookType hookType = HookType.Custom)
    {
        return new PluginHook<T>(name, hookFunc, hookType);
    }
}


public enum HookType
{
    BootStrap,
    Configuration,
    Initialize,
    Loading,
    Custom
}

public static class PluginHooks
{
    public const string Plugin_BootStrap_Hook = nameof(Plugin_BootStrap_Hook);
    public const string Plugin_Configuration_Hook = nameof(Plugin_Configuration_Hook);
    public const string Plugin_Initialize_Hook = nameof(Plugin_Initialize_Hook);
    public const string Plugin_Loading_Hook = nameof(Plugin_Loading_Hook);
}
