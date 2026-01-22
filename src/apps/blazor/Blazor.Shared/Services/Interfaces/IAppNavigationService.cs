
using FSH.Starter.Blazor.Shared.Interfaces;
using FSH.Starter.Blazor.Shared.Routing;
using Microsoft.AspNetCore.Components;

namespace FSH.Starter.BlazorShared.Services.Interfaces;
public interface IAppNavigationService : IAppNavigationManagerService<NavigationManager,AppRoute,string>
{
}
