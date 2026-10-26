
using Microsoft.JSInterop;

namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction;

public static class BlazorScriptInjector
{
    public static async ValueTask InjectScriptAsync(this IJSRuntime js, string scriptUrl)
    {
        var jsCode = @"
            (url) => {
                if (document.querySelector(`script[src='${url}']`)) return;
                const script = document.createElement('script');
                script.src = url;
                script.async = true;
                document.head.appendChild(script);
            }";

        await js.InvokeVoidAsync("eval", $"({jsCode})('{scriptUrl}')");
    }
}
