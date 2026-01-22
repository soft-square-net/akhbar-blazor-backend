using FSH.Starter.Blazor.Shared.Routing;

namespace FSH.Starter.Blazor.Shared.Interfaces;

public interface IAppRoute<T> where T : class
{
    abstract static AppRouteBase<T> Create(string app, string endpoint, string action, IDictionary<string, T> actionParameters);

    string App { get; set; }
    string Endpoint { get; set; }
    string Action { get; set; }
    IDictionary<string, T> ActionParameters { get; set; }

    abstract string Params { get; }
    //{
    //    get
    //    {
    //        StringBuilder result = new();
    //        foreach (var param in ActionParameters) {
    //            result.Append($"{param.Key}/{param.Value}/");
    //        }
    //        return result.ToString().TrimEnd('/');
    //    }
    //}

    abstract string GetRouteUrl();
}
