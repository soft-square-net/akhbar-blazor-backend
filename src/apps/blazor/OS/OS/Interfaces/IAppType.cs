using System.Reflection;

namespace FSH.Starter.Blazor.OS.Interfaces;

public interface IAppType
{
    Guid Id { get; }
    string Name { get; init; }
    string Description { get; init; }
    string Version { get; init; }
    public string IconXl { get; set; }
    public string IconMd { get; set; }
    public string IconXs { get; set; }
    string Author { get; }
    Type AppTypeClass { get; }
    string AppTypeName { get; }
    public IOsShellApp OsShell { get; }

    IEnumerable<Action> Actions { get; set; }
    IAppInstance ActiveInstances { get; set; }
    IEnumerable<IAppInstance> Instances { get; set; }
    void Open(string filePath);
    void Lunch(string filePath = "");

    void Close();
    void Minimize();  
    void Maximize();
    void Restore();
    // 
    /// <summary>
    /// When the app window gets focus
    /// </summary>
    void Focus();
    /// <summary>
    /// When the app window loses focus
    /// </summary>
    void Blur();

    void Reset();
    void Exit();

}
