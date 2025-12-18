using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.Starter.Blazor.Shared;
public interface IBlazorModule
{
    string Name { get; }
    string Description { get; }
    bool IsEnabled { get; set; } 
    bool IsLoaded { get; set; }
    bool IsInitialized { get; set; }

    Task InitializeAsync();
}
