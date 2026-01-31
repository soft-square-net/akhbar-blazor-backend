using FSH.Starter.Blazor.OS.Abstractions.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FSH.Starter.Blazor.OS.Abstractions;
public class AppInstanceBase<T>: IAppInstance<T> where T : ComponentBase
{
    public AppInstanceBase(IAppType<T> app)
    {
        App = app;
    }
    public IAppType<T> App { get; }

    public ICollection<string> Categories { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string Title { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string Description { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public IDictionary<string, object> Params { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Type ComponentType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public ICollection<string> Tags { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public void Save()
    {
        throw new NotImplementedException();
    }

    public void SaveAs(string filePath)
    {
        throw new NotImplementedException();
    }
}
