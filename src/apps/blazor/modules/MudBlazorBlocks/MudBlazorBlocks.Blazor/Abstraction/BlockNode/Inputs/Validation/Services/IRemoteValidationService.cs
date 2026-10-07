namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction.BlockNode.Services;

// IRemoteValidationService.cs
using System.Threading;
using System.Threading.Tasks;

public interface IRemoteValidationService
{
    Task<bool> ValidateRemoteAsync(
        string ruleName, 
        string endpointUrl, 
        object? value, 
        CancellationToken cancellationToken);
}
