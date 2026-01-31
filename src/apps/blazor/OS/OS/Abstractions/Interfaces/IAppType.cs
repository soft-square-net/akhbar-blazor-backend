using Microsoft.AspNetCore.Components;

namespace FSH.Starter.Blazor.OS.Abstractions.Interfaces;

public interface IAppType<T> where T : ComponentBase
{
    Guid Id { get; }
    string Name { get; init; }
    string Description { get; init; }
    string Version { get; init; }
    string Icon { get; set; }
    string Route { get; set; } // e.g., "/App/Calculator"
    bool IsOpen { get; set; }
    string Author { get; }
    Type AppTypeClass { get; }
    string AppTypeName { get; }
    // Reference to the OS Shell
    // public IOsShell<TDialog, TOptions, TResult> OsShell { get; }  

    IEnumerable<Action> Actions { get; set; }
    IAppInstance<T> ActiveInstances { get; set; }
    IEnumerable<IAppInstance<T>> Instances { get; set; }
    void Open(string filePath);
    void Lunch(string filePath = "");
    void GeneratrLuncher(string savePath,string? filePath = "");

    void Close();

    void Exit();


    /* Transfer those variables to IOsWindow
     * void Minimize();  
     * void Maximize();
     * void Restore();
     * 

    /// <summary>
    /// When the app window gets focus
    /// </summary>
    void Focus();
    /// <summary>
    /// When the app window loses focus
    /// </summary>
    void Blur();

    void Reset(); */

}
