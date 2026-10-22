using BlazorModularEngine.Core.Hooks;
using BlazorModularEngine.Core.UI;
using BlazorModularEngine.Core.UI.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;

namespace MudBlazorLayout;

public class MudBlazorLayoutModule: ILayoutModule
{
    public string LayoutId => "mudblazor";
    public string DisplayName => "MudBlazor Theme";
    public string ModuleName => "MudBlazorLayout";
    public string Version => "1.0.0";
    public Type LayoutComponentType => typeof(MudBlazorizerLayout);
    public string[] RequiredStylesheets => new[] { 
        "_content/MudBlazor/MudBlazor.min.css" ,
        "https://fonts.googleapis.com/css2?family=Roboto:ital,wght@0,100..900;1,100..900&display=swap"
    };

    public string[] RequiredScripts => new[] { "_content/MudBlazor/MudBlazor.min.js" }; 
    // Implement your IBlazorModule startup/registration hooks here
    public void ConfigureServices(IServiceCollection services)
    {
        // services.AddMudServices(); // Only loads MudBlazor services when this layout is active!
        services.AddMudServices(configuration =>
        {
            configuration.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
            configuration.SnackbarConfiguration.NewestOnTop = false;
            configuration.SnackbarConfiguration.ShowCloseIcon = true;
            configuration.SnackbarConfiguration.VisibleStateDuration = 3000;
            configuration.SnackbarConfiguration.HideTransitionDuration = 100;
            configuration.SnackbarConfiguration.ShowTransitionDuration = 100;
            configuration.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
            // configuration.PopoverOptions.OverflowBehavior = OverflowBehavior.FlipNever;
        });
    }

    public void RegisterHooks(PluginHookManager hookManager)
    {
        // throw new NotImplementedException();
    }

    public void RegisterUI(PluginSlotRegistry slotRegistry)
    {
        // throw new NotImplementedException();
    }

    public async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        // throw new NotImplementedException();
    }

    public Type GetRootComponentType() => typeof(MudBlazorizerLayout);


}
