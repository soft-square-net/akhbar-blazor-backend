using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace FSH.Starter.BlazorShared.Layout;

public sealed class ScriptsContent : ComponentBase
{
    public ScriptsContent() { }

    //
    // Summary:
    //     Gets or sets the content to be rendered in Microsoft.AspNetCore.Components.Web.HeadOutlet
    //     instances.
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder) { 
        if (ChildContent != null)
        {
            builder.OpenElement(0, "div");
            // builder.AddComponentParameter(1, "src", "/stc/to/script/filename.js");
            builder.AddContent(1, ChildContent);
            builder.CloseElement();
        }
    }
}
