using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSH.Starter.Blazor.OS.Services.Interfaces;

namespace FSH.Starter.Blazor.OS.Interfaces;
public interface IOsShellApp
{
    IAppManagerService AppManagerService { get; }
}
