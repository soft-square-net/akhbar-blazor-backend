using System.Diagnostics.CodeAnalysis;

// namespace AkhbarBlazorBackend.Application.Interfaces.Services.Storage
namespace FSH.Framework.Core.Storage.EventArgs;
    [ExcludeFromCodeCoverage]
    public class ChangedEventArgs
    {
        public string Key { get; set; }
        public object OldValue { get; set; }
        public object NewValue { get; set; }
    }

