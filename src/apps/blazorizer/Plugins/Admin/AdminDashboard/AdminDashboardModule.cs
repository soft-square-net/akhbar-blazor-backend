using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminDashboard.Components;
using AdminDashboard.Services;
using AdminDashboard.Services.Interfaces;
using BlazorModularEngine.Core.Common.Interfaces;
using BlazorModularEngine.Core.Hooks;
using BlazorModularEngine.Core.Models;
using BlazorModularEngine.Core.UI;
using BlazorModularEngine.Core.UI.Theme;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;

namespace AdminDashboard;

public class AdminDashboardModule : IBlazorModule
{
    public string ModuleName => "AdminDashboard";
    public string Version => "1.0.0";

    public void ConfigureServices(IServiceCollection services)
    {
        // Register plugin-specific service into DI
        services.AddTransient<IAdminStatsService, AdminStatsService>();
    }

    public void RegisterHooks(PluginHookManager hookManager)
    {
        // Register WordPress-style action hook
        hookManager.AddAction("on_admin_module_loaded", args =>
        {
            Console.WriteLine("Admin Dashboard Module successfully loaded and initialized!");
        });
    }

    public void RegisterUI(PluginSlotRegistry slotRegistry)
    {
        // Register navigation link into sidebar slot
        slotRegistry.RegisterComponent("theme-sidebar", typeof(AdminSidebarNav));
    }

    public Task InitializeAsync(IServiceProvider serviceProvider)
    {
        // Trigger hook upon initialization
        var hookManager = serviceProvider.GetService<PluginHookManager>();
        hookManager?.DoAction("on_admin_module_loaded", ModuleName);

        // Dynamically install and apply a custom theme via ThemeManager at runtime!
        var themeManager = serviceProvider.GetService<ThemeManager>();
        if (themeManager != null)
        {
            themeManager.InstallAndApplyTheme(new ThemeTemplate
            {
                Id = "neon-cyberpunk",
                ThemeName = "Cyberpunk Neon (Plugin Theme)",
                Framework = CssFrameworkType.Tailwind,
                LayoutMode = LayoutStyleMode.StandardLeftSidebar,
                RootContainerClass = "min-h-screen flex flex-col bg-zinc-950 text-zinc-100 font-mono",
                HeaderClass = "bg-zinc-900 border-b border-cyan-500/30 shadow-lg shadow-cyan-500/10 z-10 sticky top-0",
                SidebarClass = "w-full md:w-64 bg-zinc-900 border-r border-cyan-500/30 p-4 space-y-4 shrink-0",
                MainContentClass = "flex-1 p-6 md:p-8 max-w-7xl mx-auto w-full",
                FooterClass = "bg-zinc-900 border-t border-cyan-500/30 py-4 px-6 text-center text-sm text-zinc-400 mt-auto"
            });
        }

        return Task.CompletedTask;
    }

    // Helper method expected by host loader to get root component type
    public Type GetRootComponentType() => typeof(AdminDashboardLayout);

    public IEnumerable<MenuItemModel> GetMenuItems()
    {
        yield return new MenuItemModel
        {
            Title = "Management",
            Icon = Icons.Material.Filled.Business,
            Region = "sidebar",
            Order = 10,
            Children = new List<MenuItemModel>
            {
                new MenuItemModel
                {
                    Title = "Human Resources",
                    Icon = Icons.Material.Filled.People,
                    Children = new List<MenuItemModel>
                    {
                        new MenuItemModel { Title = "Employees", Href = "/hr/employees", Icon = Icons.Material.Filled.Badge },
                        new MenuItemModel
                        {
                            Title = "Payroll & Benefits",
                            Icon = Icons.Material.Filled.AttachMoney,
                            Children = new List<MenuItemModel>
                            {
                                new MenuItemModel { Title = "Salaries", Href = "/hr/payroll/salaries", Icon = Icons.Material.Filled.Money },
                                new MenuItemModel { Title = "Insurance", Href = "/hr/payroll/insurance", Icon = Icons.Material.Filled.Security }
                            }
                        }
                    }
                },
                new MenuItemModel
                {
                    Title = "Inventory",
                    Icon = Icons.Material.Filled.Warehouse,
                    Children = new List<MenuItemModel>
                    {
                        new MenuItemModel { Title = "Warehouses", Href = "/inventory/warehouses", Icon = Icons.Material.Filled.House },
                        new MenuItemModel { Title = "Stock Items", Href = "/inventory/items", Icon = Icons.Material.Filled.Inventory }
                    }
                }
            }
        };
    }
}
