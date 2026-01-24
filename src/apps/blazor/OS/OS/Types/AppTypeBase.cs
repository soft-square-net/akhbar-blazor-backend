

using FSH.Starter.Blazor.OS.Interfaces;

namespace FSH.Starter.Blazor.OS.Types;
public abstract class AppTypeBase : IAppType
{
    private readonly IOsShellApp _os;

    protected AppTypeBase(IOsShellApp os)
    {
        _os = os;
    }
    protected static AppTypeBase Create<T>() where T : AppTypeBase, new()
    {
        return new T();
    }

   

    public abstract Guid Id { get; }
    public abstract string Name { get; init; }
    public abstract string Description { get; init; }
    public abstract string Version { get; init; }
    public abstract string Author { get; }
    public abstract Type AppTypeClass { get; }
    public abstract string AppTypeName { get; }
   public string IconXl { get; set; }
    public string IconMd { get; set; }
    public string IconXs { get; set; }
    public IEnumerable<Action> Actions { get; set; }
    public IAppInstance ActiveInstances { get; set; }
    public IEnumerable<IAppInstance> Instances { get; set; }

    public IOsShellApp OsShell => _os;

    public abstract void Open(string filePath);

    public void Lunch(string filePath = "")
    {
        throw new NotImplementedException();
    }

    public void Close()
    {
        throw new NotImplementedException();
    }

    public void Minimize()
    {
        throw new NotImplementedException();
    }

    public void Maximize()
    {
        throw new NotImplementedException();
    }

    public void Restore()
    {
        throw new NotImplementedException();
    }

    public void Focus()
    {
        throw new NotImplementedException();
    }

    public void Blur()
    {
        throw new NotImplementedException();
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public void Exit()
    {
        throw new NotImplementedException();
    }
}
