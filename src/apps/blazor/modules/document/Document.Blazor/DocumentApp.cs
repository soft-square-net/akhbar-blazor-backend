
using FSH.Starter.Blazor.OS.Abstractions;
using FSH.Starter.Blazor.OS.Interfaces;
using MudBlazor;
using MudBlazor.Extensions.Components;
using MudBlazor.Extensions.Options;

namespace FSH.Starter.Blazor.Modules.Document.Blazor;
public class DocumentApp : AppTypeBase<MudExDialog, DialogOptionsEx, DialogResult>
{
    public DocumentApp(IOsShell<MudExDialog, DialogOptionsEx, DialogResult> os) : base(os)
    {
    }

    public override Guid Id => throw new NotImplementedException();

    public override string Name { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }
    public override string Description { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }
    public override string Version { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }

    public override string Author => throw new NotImplementedException();

    public override Type AppTypeClass => throw new NotImplementedException();

    public override string AppTypeName => throw new NotImplementedException();

    public override void Open(string filePath)
    {
        throw new NotImplementedException();
    }
}
