using System.Diagnostics.CodeAnalysis;

// namespace AkhbarBlazorBackend.Application.Interfaces.Services.Storage
namespace FSH.Framework.Core.Storage.EventArgs;

[ExcludeFromCodeCoverage]
public class ChangingEventArgs : ChangedEventArgs
{
    public bool Cancel { get; set; }
}
