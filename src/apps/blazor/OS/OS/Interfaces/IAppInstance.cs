using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.Starter.Blazor.OS.Interfaces;
public interface IAppInstance : IDisposable
{
    IAppType App { get; }
    ICollection<string> Categories { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public IDictionary<string,object> Params { get; set; }
    public Type ComponentType { get; set; }
    public Type OsShell { get; set; }

    void Save();
    void SaveAs(string filePath);
}
