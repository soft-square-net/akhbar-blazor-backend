
using FSH.Starter.Blazor.OS.Interfaces;

namespace FSH.Starter.Blazor.OS.Types;
public class AppInstanceBase : IAppInstance
{
    public AppInstanceBase(IAppType app)
    {
        App = app;
    }
    public IAppType App { get; }

    public ICollection<string> Categories { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string Title { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string Description { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public IDictionary<string, object> Params { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Type ComponentType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
