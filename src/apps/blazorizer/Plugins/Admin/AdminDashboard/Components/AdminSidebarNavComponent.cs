using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AdminDashboard.Components;


public class AdminSidebarNavComponent : ComponentBase
{
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        int seq = 0;
        builder.OpenElement(seq++, "a");
        builder.AddAttribute(seq++, "href", "/modules/AdminDashboard");
        builder.AddAttribute(seq++, "class", "flex items-center gap-3 px-4 py-2.5 text-sm font-medium text-slate-700 hover:bg-indigo-50 hover:text-indigo-600 rounded-lg transition-colors");
        {
            builder.OpenElement(seq++, "span");
            builder.AddAttribute(seq++, "class", "w-2 h-2 rounded-full bg-indigo-500");
            builder.CloseElement();
            builder.AddContent(seq++, "Admin Portal");
        }
        builder.CloseElement();
    }
}
