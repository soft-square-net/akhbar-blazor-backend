

using FSH.Starter.Blazor.OS.Abstractions;
using FSH.Starter.Blazor.OS.Interfaces;
using FSH.Starter.Blazor.OS.Services;
using FSH.Starter.Blazor.OS.Services.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.Blazor.OS;
public static class OsExtentionMethods
{
    public static IOsShell<TDialog, TOptions, TResult> ConfigureGuiOsSystem<TDialog, TOptions, TResult>(this IOsShell<TDialog, TOptions, TResult> osShell, IServiceCollection services)
    {
        // services.AddScoped<IOsShell, OsShell>();
        services.AddScoped<IAppManagerService<TDialog, TOptions, TResult>, AppManagerService<TDialog, TOptions, TResult>>();
        return osShell;
    }

    public static IOsShell<TDialog, TOptions, TResult> UseOsSystem<TDialog, TOptions, TResult>(this IOsShell<TDialog, TOptions, TResult> osShell, WebAssemblyHostBuilder app)
    {
        return osShell;
    }
}
