using FSH.Starter.Blazor.Shared.Interfaces;

namespace FSH.Starter.Blazor.Shared.Routing;
public abstract class AppNavigationManagerBaseService<T,TAppRoute,TParam>(T navigationManager): IAppNavigationManagerService<T,TAppRoute,TParam> 
    where T : class 
    where TParam : class 
    where TAppRoute : AppRouteBase<TParam> 
{
    private readonly T _navigationManager = navigationManager;
    private readonly IList<TAppRoute> _appRoutes = new List<TAppRoute>();

    public T AppNavigationManager => _navigationManager;

    public IList<TAppRoute> GetAllRoutes(string appName) => _appRoutes;

    public IList<TAppRoute> GetAppRoutes(string appName) => _appRoutes.Where(r => r.App == appName).ToList();

    public abstract void NavigateTo(TAppRoute route);

    public void RegisterAppRoute(TAppRoute route)
    {
        _appRoutes.Add(route);
    }
}
