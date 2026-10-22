namespace BlazorModularEngine.Core.UI.Interfaces;

public interface ILayoutManager
{
    IReadOnlyCollection<ILayoutModule> AvailableLayouts { get; }
    ILayoutModule CurrentLayout { get; }
    event Action OnLayoutChanged;
    Task SetActiveLayoutAsync(string layoutId);
}
