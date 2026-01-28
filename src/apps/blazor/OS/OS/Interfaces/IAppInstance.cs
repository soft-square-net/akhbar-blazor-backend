
using Microsoft.AspNetCore.Components;

namespace FSH.Starter.Blazor.OS.Interfaces;
public interface IAppInstance<T>: IDisposable where T : ComponentBase
{
    IAppType<T> App { get; }
    ICollection<string> Tags { get; set; }
    public string Title { get; set; }
    public IDictionary<string,object> Params { get; set; }

}
