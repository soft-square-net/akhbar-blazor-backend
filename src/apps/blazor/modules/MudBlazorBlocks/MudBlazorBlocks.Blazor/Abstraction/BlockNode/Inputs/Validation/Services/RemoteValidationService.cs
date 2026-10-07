namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction.BlockNode.Services;

// Example Mock Implementation
public class RemoteValidationService : IRemoteValidationService
{
    public async Task<bool> ValidateRemoteAsync(
        string ruleName, 
        string endpointUrl, 
        object? value, 
        CancellationToken cancellationToken)
    {
        // Simulate network latency
        await Task.Delay(400, cancellationToken);

        if (ruleName == "CheckUsernameAvailability")
        {
            var username = value?.ToString();
            // Simulate 'admin' and 'john_doe' being taken
            if (string.Equals(username, "admin", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(username, "john_doe", StringComparison.OrdinalIgnoreCase))
            {
                return false; // Invalid
            }
        }

        return true; // Valid
    }
}
