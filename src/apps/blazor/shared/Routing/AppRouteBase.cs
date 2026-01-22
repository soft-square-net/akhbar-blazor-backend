using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSH.Starter.Blazor.Shared.Interfaces;

namespace FSH.Starter.Blazor.Shared.Routing;
public abstract class AppRouteBase<T>:IAppRoute<T> where T : class
{
    protected AppRouteBase() 
    {
        ActionParameters = new Dictionary<string, T>();
    }
    public static AppRouteBase<T> Create(string app, string endpoint, string action, IDictionary<string,T> actionParameters)
    {
        var route = (AppRouteBase<T>)Activator.CreateInstance(typeof(AppRouteBase<T>), true)!;
        route.App = app;
        route.Endpoint = endpoint;
        route.Action = action;
        route.ActionParameters = actionParameters;
        return route;
    }
    public virtual string App { get; set; }
    public string Endpoint { get; set; }
    public string Action { get; set; }
    public IDictionary<string,T> ActionParameters { get; set; }

    public abstract string Params { get;}


    public abstract string GetRouteUrl();
    //{
    //    return $"{App}/{Endpoint}/{Action}/{Params}".TrimEnd('/');
    //}



}
