using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FSH.Starter.BlazorShared.Components.Common;

public static class DialogServiceExtensions
{
    public static async Task<DialogResult> ShowModalAsync<TDialog>(this IDialogService dialogService, DialogParameters parameters)
        where TDialog : ComponentBase =>
        await (await dialogService.ShowModal<TDialog>(parameters)!).Result!;

    public async static Task<IDialogReference> ShowModal<TDialog>(this IDialogService dialogService, DialogParameters parameters)
        where TDialog : ComponentBase
    {
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

        return await dialogService.ShowAsync<TDialog>(string.Empty, parameters, options);
    }
}
