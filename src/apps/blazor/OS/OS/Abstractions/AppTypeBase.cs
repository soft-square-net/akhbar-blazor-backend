using FSH.Starter.Blazor.OS.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FSH.Starter.Blazor.OS.Abstractions;
public abstract class AppTypeBase<TDialog, TOptions, TResult> : IAppType<ComponentBase>
{
    private readonly IOsShell<TDialog, TOptions, TResult> _os;

    protected AppTypeBase(IOsShell<TDialog, TOptions, TResult> os)
    {
        _os = os;
    }
    protected static AppTypeBase<TDialog, TOptions, TResult> Create<T>() where T : AppTypeBase<TDialog, TOptions, TResult>, new()
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
    public IAppInstance<ComponentBase> ActiveInstances { get; set; }
    public IEnumerable<IAppInstance<ComponentBase>> Instances { get; set; }

    public IOsShell<TDialog, TOptions, TResult> OsShell => _os;

    public string Icon { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string Route { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool IsOpen { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public abstract void Open(string filePath);

    public void Lunch(string filePath = "")
    {
        throw new NotImplementedException();
    }

    public void GeneratrLuncher(string savePath, string? filePath = "")
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
