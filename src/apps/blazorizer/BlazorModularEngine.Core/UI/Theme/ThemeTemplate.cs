using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorModularEngine.Core.UI.Theme;

/// <summary>
/// Configuration model for a complete theme definition across different frameworks.
/// </summary>
public class ThemeTemplate
{
    public string Id { get; set; } = "tailwind-default";
    public string ThemeName { get; set; } = "Tailwind CSS (Modern Light)";
    public CssFrameworkType Framework { get; set; } = CssFrameworkType.Tailwind;
    public LayoutStyleMode LayoutMode { get; set; } = LayoutStyleMode.StandardLeftSidebar;

    // Slot Mapping
    public string HeaderSlotName { get; set; } = "theme-header";
    public string SidebarSlotName { get; set; } = "theme-sidebar";
    public string FooterSlotName { get; set; } = "theme-footer";

    // Style Tokens for Visual Customization
    public string RootContainerClass { get; set; } = "min-h-screen flex flex-col bg-slate-50 text-slate-900 font-sans transition-colors duration-300";
    public string HeaderClass { get; set; } = "bg-white border-b border-slate-200 shadow-xs z-10 sticky top-0";
    public string HeaderInnerClass { get; set; } = "max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 h-16 flex items-center justify-between w-full";
    public string SidebarClass { get; set; } = "w-full md:w-64 bg-white border-r border-slate-200 p-4 space-y-4 shrink-0 transition-all duration-300";
    public string MainContentClass { get; set; } = "flex-1 p-6 md:p-8 max-w-7xl mx-auto w-full";
    public string FooterClass { get; set; } = "bg-white border-t border-slate-200 py-4 px-6 text-center text-sm text-slate-500 mt-auto";
}
