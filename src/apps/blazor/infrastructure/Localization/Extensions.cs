using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Shared.Enums;

namespace FSH.Starter.Blazor.Infrastructure.Localization;

public static class Extensions
{
    public static IServiceCollection AddFSHLocalization(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        services.AddLocalization(options => options.ResourcesPath = "Resources");
        // services.AddRazorComponents().AddInteractiveServerComponents(); // or AddInteractiveWebAssemblyComponents
        return services;
    }
    public static async Task<WebAssemblyHost> UseFSHLocalization(this WebAssemblyHost app)
    {
        var jsRuntime = app.Services.GetRequiredService<IJSRuntime>();
        var result = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "blazorCulture");
        var cultureName = result ?? FSHLang.Values[0];

        var culture = new CultureInfo(cultureName);

        // 3. Set runtime culture default threads
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;

        return app;
    }
}
