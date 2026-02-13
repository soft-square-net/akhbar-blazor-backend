using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.Starter.Blazor.OS.Abstractions.Interfaces;
public interface ITaskbar<TRESULT>
{
    IStartMenu StartMenu { get;}
    ISearchMenu SearchMenu { get;}
    IEnumerable<IServiceApp> Servicebar { get;}
    IEnumerable<ITaskbarAppLuncher<TRESULT>> Items { get; set; }
}
