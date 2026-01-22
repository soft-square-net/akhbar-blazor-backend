
using FSH.Starter.Blazor.Shared.Routing;

namespace FSH.Starter.Blazor.Shared.Interfaces;

public interface IAppNavigationManagerService<T,TAppRoute,TRouteParam> 
    where T : class
    where TAppRoute : AppRouteBase<TRouteParam>
    where TRouteParam : class
{
    T AppNavigationManager {  get; }
    void RegisterAppRoute(TAppRoute route);
    
    IList<TAppRoute> GetAppRoutes(string appName);
    IList<TAppRoute> GetAllRoutes(string appName);

    void NavigateTo(TAppRoute route);
}
