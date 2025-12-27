namespace FSH.Starter.BlazorShared.interfaces;
public interface IModuleMenu
{
    string Name { get; }
    string Title { get; }
    string Description { get; }
    int Order { get; }
}
