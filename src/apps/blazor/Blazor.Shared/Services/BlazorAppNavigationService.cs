using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSH.Starter.Blazor.Shared.Routing;
using FSH.Starter.BlazorShared.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FSH.Starter.BlazorShared.Services;
public class BlazorAppNavigationService : AppNavigationManagerBaseService<NavigationManager, AppRoute, string>, IAppNavigationService
{
    public BlazorAppNavigationService(NavigationManager navigationManager) : base(navigationManager)
    {
    }
    public override void NavigateTo(AppRoute route)
    {
        AppNavigationManager.NavigateTo(route.GetRouteUrl());
    }
}

