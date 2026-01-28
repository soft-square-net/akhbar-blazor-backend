using Microsoft.AspNetCore.Components;

namespace FSH.Starter.Blazor.Modules.DesktopLayout.Blazor.Layout;
public partial class AppRouter
{
    [Parameter] public RenderFragment ChildContent { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    [CascadingParameter(Name = "AppRouteData")]
    private RouteData? CurrentRouteData { get; set; }

    [SupplyParameterFromQuery]
    public string? Query { get; set; }
    [Parameter]
    public string? AppName { get; set; }
    [Parameter]
    public string? ActionName { get; set; }
    [Parameter]
    public string? PageRoute { get; set; }
    [Parameter]
    public string? Path { get; set; }

    private string? _fullPath;


    override protected void OnParametersSet()
    {
        base.OnParametersSet();

        _fullPath = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        
        if (CurrentRouteData.RouteValues.ContainsKey("AppName") && CurrentRouteData.RouteValues.ContainsKey("ActionName"))
            _fullPath = $"/app/{CurrentRouteData.RouteValues["AppName"]}/action/{CurrentRouteData.RouteValues["ActionName"]}?{Query}";
        // You can use the parameters here as needed
        // For example, log them or use them to determine what to display
    }
}
