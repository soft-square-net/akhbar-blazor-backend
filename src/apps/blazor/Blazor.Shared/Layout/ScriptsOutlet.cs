using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Sections;
using Microsoft.JSInterop;

namespace FSH.Starter.BlazorShared.Layout;
public sealed class ScriptsOutlet : ComponentBase
{
    // private const string GetAndRemoveExistingTitle = "Blazor._internal.PageTitle.getAndRemoveExistingTitle";

    internal static readonly object ScriptsSectionId = new object();

    // internal static readonly object TitleSectionId = new object();

    // private string _defaultTitle;

    [Inject]
    private IJSRuntime JSRuntime { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // _defaultTitle = await JSRuntimeExtensions.InvokeAsync<string>(JSRuntime, "Blazor._internal.PageTitle.getAndRemoveExistingTitle", Array.Empty<object>());
            StateHasChanged();
        }
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        //builder.OpenComponent<SectionOutlet>(0);
        //builder.AddComponentParameter(1, "SectionId", TitleSectionId);
        //builder.CloseComponent();
        //if (!string.IsNullOrEmpty(_defaultTitle))
        //{
        //    builder.OpenComponent<SectionContent>(2);
        //    builder.AddComponentParameter(3, "SectionId", TitleSectionId);
        //    builder.AddComponentParameter(4, "IsDefaultContent", true);
        //    builder.AddComponentParameter(5, "ChildContent", new RenderFragment(BuildDefaultTitleRenderTree));
        //    builder.CloseComponent();
        //}

        builder.OpenComponent<SectionOutlet>(0);
        builder.AddComponentParameter(1, "SectionId", ScriptsSectionId);
        builder.CloseComponent();

        builder.OpenComponent<SectionContent>(2);
        builder.AddComponentParameter(3, "SectionId", ScriptsSectionId);
        builder.AddComponentParameter(4, "IsDefaultContent", true);
        builder.AddComponentParameter(5, "ChildContent", new RenderFragment(BuildDefaultScriptsRenderTree));
        builder.CloseComponent();
    }

    private void BuildDefaultScriptsRenderTree(RenderTreeBuilder builder)
    {
        //builder.OpenElement(6, "script");
        //builder.AddComponentParameter(7, "src", "/stc/to/script/filename.js");
        //// builder.AddContent(1, "");
        //builder.CloseElement();
    }
}
