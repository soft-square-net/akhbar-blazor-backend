
using BlazorModularEngine.Core.Common.Interfaces;
using BlazorModularEngine.Core.Common.Plugins;
using BlazorModularEngine.Core.Hooks;
using BlazorModularEngine.Core.UI;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorModularEngine.Core.Examples.Modules;

public class MyAwesomePlugin : IBlazorModule
{
    public string ModuleName => "AwesomePlugin";
    public string Version => "1.0.0";

    public void ConfigureServices(IServiceCollection services) { /* Register DI */ }

    public void RegisterHooks(PluginHookManager hookManager)
    {
        // WordPress-style Action (Event listener)
        hookManager.AddAction("on_user_login", args => {
            var username = args[0] as string;
            Console.WriteLine($"Plugin handled login for: {username}");
        });

        // WordPress-style Filter (Data modifier)
        hookManager.AddFilter("format_invoice_title", (value, context) => {
            return $"[Verified] {value}";
        });
    }

    public void RegisterUI(PluginSlotRegistry slotRegistry)
    {
        // Register component to render automatically inside host layout slot "sidebar-menu"
        slotRegistry.RegisterComponent("sidebar-menu", typeof(AwesomePluginMenuComponent));
    }

    public Task InitializeAsync(IServiceProvider sp) => Task.CompletedTask;

    public Type GetRootComponentType()
    {
        throw new NotImplementedException();
    }
}

public class AwesomePluginMenuComponent : PluginComponentBase
{
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        int seq = 0;
        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "class", "p-4 bg-indigo-50 rounded-lg shadow-md border border-indigo-200 space-y-4");
        {
            builder.OpenElement(seq++, "h3");
            builder.AddAttribute(seq++, "class", "text-lg font-semibold text-indigo-800");
            builder.AddContent(seq++, "Awesome Plugin Menu");
            builder.CloseElement();
            builder.OpenElement(seq++, "ul");
            builder.AddAttribute(seq++, "class", "space-y-2");
            {
                builder.OpenElement(seq++, "li");
                builder.OpenElement(seq++, "a");
                builder.AddAttribute(seq++, "href", "/awesome-plugin/dashboard");
                builder.AddAttribute(seq++, "class", "text-indigo-600 hover:underline");
                builder.AddContent(seq++, "Dashboard");
                builder.CloseElement();
                builder.CloseElement();
                builder.OpenElement(seq++, "li");
                builder.OpenElement(seq++, "a");
                builder.AddAttribute(seq++, "href", "/awesome-plugin/settings");
                builder.AddAttribute(seq++, "class", "text-indigo-600 hover:underline");
                builder.AddContent(seq++, "Settings");
                builder.CloseElement();
                builder.CloseElement();
            }
            builder.CloseElement(); // ul
        }
        builder.CloseElement(); // div
    }
}
