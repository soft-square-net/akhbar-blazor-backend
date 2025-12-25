using MudBlazor;

namespace FSH.Starter.BlazorShared;
public abstract partial class MobulePageBase: ComponentBaseWithState
{
    protected abstract string Path { get; set; } 
    //protected override Task OnParametersSetAsync()
    //{
    //    return base.OnParametersSetAsync();
    //}

    //protected override Task OnInitializedAsync()
    //{
    //    return base.OnInitializedAsync();
    //}

    //protected override Task OnAfterRenderAsync(bool firstRender)
    //{
    //    return base.OnAfterRenderAsync(firstRender);
    //}
}
