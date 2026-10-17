using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using BlazorModularEngine.Core;
using BlazorModularEngine.Core.UI.Theme;

namespace BlazorModularEngine.Core.UI.Theme;


public class DynamicThemeLayout : LayoutComponentBase, IDisposable
{
    [Inject] private ThemeManager ThemeManager { get; set; } = default!;
    [Inject] private PluginSlotRegistry SlotRegistry { get; set; } = default!;

    protected override void OnInitialized()
    {
        ThemeManager.OnThemeChanged += HandleThemeChanged;
    }

    private void HandleThemeChanged()
    {
        InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        if (ThemeManager != null)
        {
            ThemeManager.OnThemeChanged -= HandleThemeChanged;
        }
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var template = ThemeManager.CurrentTemplate;

        // Strategy selector rendering clean RenderFragments instead of tedious seq++ code
        RenderFragment layoutContent = template.Framework switch
        {
            CssFrameworkType.Bootstrap => BuildBootstrapLayout(template),
            CssFrameworkType.MudBlazor => BuildMudBlazorLayout(template),
            CssFrameworkType.Tailwind or _ => BuildTailwindLayout(template)
        };

        layoutContent(builder);
    }

    private RenderFragment BuildTailwindLayout(ThemeTemplate template) => builder =>
    {
        int seq = 0;
        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "class", template.RootContainerClass);
        builder.AddAttribute(seq++, "data-framework", "tailwind");
        {
            // Header Region
            builder.OpenElement(seq++, "header");
            builder.AddAttribute(seq++, "class", template.HeaderClass);
            {
                builder.OpenElement(seq++, "div");
                builder.AddAttribute(seq++, "class", template.HeaderInnerClass);
                RenderSlotRegion(builder, template.HeaderSlotName, ref seq);
                builder.CloseElement();
            }
            builder.CloseElement();

            // Body & Sidebar Wrapper
            builder.OpenElement(seq++, "div");
            builder.AddAttribute(seq++, "class", "flex-1 flex flex-col md:flex-row w-full");
            {
                if (template.LayoutMode == LayoutStyleMode.StandardLeftSidebar)
                {
                    RenderSidebar(builder, template, ref seq);
                }

                builder.OpenElement(seq++, "main");
                builder.AddAttribute(seq++, "class", template.MainContentClass);
                if (Body != null) builder.AddContent(seq++, Body);
                builder.CloseElement();

                if (template.LayoutMode == LayoutStyleMode.RightSidebar)
                {
                    RenderSidebar(builder, template, ref seq);
                }
            }
            builder.CloseElement();

            // Footer Region
            builder.OpenElement(seq++, "footer");
            builder.AddAttribute(seq++, "class", template.FooterClass);
            RenderSlotRegion(builder, template.FooterSlotName, ref seq);
            builder.CloseElement();
        }
        builder.CloseElement();
    };

    private RenderFragment BuildBootstrapLayout(ThemeTemplate template) => builder =>
    {
        int seq = 0;
        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "class", template.RootContainerClass);
        builder.AddAttribute(seq++, "data-framework", "bootstrap");
        {
            // Navbar Header
            builder.OpenElement(seq++, "nav");
            builder.AddAttribute(seq++, "class", template.HeaderClass);
            {
                builder.OpenElement(seq++, "div");
                builder.AddAttribute(seq++, "class", template.HeaderInnerClass);
                RenderSlotRegion(builder, template.HeaderSlotName, ref seq);
                builder.CloseElement();
            }
            builder.CloseElement();

            // Main Body Container
            builder.OpenElement(seq++, "div");
            builder.AddAttribute(seq++, "class", "container-fluid flex-grow-1 d-flex flex-row p-0");
            {
                if (template.LayoutMode == LayoutStyleMode.StandardLeftSidebar)
                {
                    RenderSidebar(builder, template, ref seq);
                }

                builder.OpenElement(seq++, "main");
                builder.AddAttribute(seq++, "class", template.MainContentClass);
                if (Body != null) builder.AddContent(seq++, Body);
                builder.CloseElement();

                if (template.LayoutMode == LayoutStyleMode.RightSidebar)
                {
                    RenderSidebar(builder, template, ref seq);
                }
            }
            builder.CloseElement();

            // Footer
            builder.OpenElement(seq++, "footer");
            builder.AddAttribute(seq++, "class", template.FooterClass);
            RenderSlotRegion(builder, template.FooterSlotName, ref seq);
            builder.CloseElement();
        }
        builder.CloseElement();
    };

    private RenderFragment BuildMudBlazorLayout(ThemeTemplate template) => builder =>
    {
        int seq = 0;
        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "class", template.RootContainerClass + " mud-appbar-leading-dense");
        builder.AddAttribute(seq++, "data-framework", "mudblazor");
        {
            // AppBar Header
            builder.OpenElement(seq++, "header");
            builder.AddAttribute(seq++, "class", template.HeaderClass);
            {
                builder.OpenElement(seq++, "div");
                builder.AddAttribute(seq++, "class", "mud-toolbar mud-toolbar-appbar px-6 d-flex align-center justify-space-between w-100");
                RenderSlotRegion(builder, template.HeaderSlotName, ref seq);
                builder.CloseElement();
            }
            builder.CloseElement();

            // Drawer & Content Wrapper
            builder.OpenElement(seq++, "div");
            builder.AddAttribute(seq++, "class", "d-flex flex-row flex-grow-1");
            {
                if (template.LayoutMode == LayoutStyleMode.StandardLeftSidebar)
                {
                    RenderSidebar(builder, template, ref seq);
                }

                builder.OpenElement(seq++, "main");
                builder.AddAttribute(seq++, "class", template.MainContentClass);
                if (Body != null) builder.AddContent(seq++, Body);
                builder.CloseElement();
            }
            builder.CloseElement();

            // Footer
            builder.OpenElement(seq++, "footer");
            builder.AddAttribute(seq++, "class", template.FooterClass);
            RenderSlotRegion(builder, template.FooterSlotName, ref seq);
            builder.CloseElement();
        }
        builder.CloseElement();
    };

    private void RenderSidebar(RenderTreeBuilder builder, ThemeTemplate template, ref int seq)
    {
        builder.OpenElement(seq++, "aside");
        builder.AddAttribute(seq++, "class", template.SidebarClass);
        RenderSlotRegion(builder, template.SidebarSlotName, ref seq);
        builder.CloseElement();
    }

    private void RenderSlotRegion(RenderTreeBuilder builder, string slotName, ref int seq)
    {
        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "class", "d-flex align-center w-100 gap-4");
        {
            var componentTypes = SlotRegistry.GetComponentsForSlot(slotName);
            foreach (var componentType in componentTypes)
            {
                builder.OpenComponent(seq++, componentType);
                builder.CloseComponent();
            }
        }
        builder.CloseElement();
    }
}
