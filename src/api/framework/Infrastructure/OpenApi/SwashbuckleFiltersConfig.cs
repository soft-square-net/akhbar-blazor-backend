using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.Framework.Infrastructure.OpenApi;
public class SwashbuckleFiltersConfig
{
    public string AssembliyPrefix { get; set; }
    public Filters Filters { get; set; }
}

public class Filters
{
    public bool Enabled { get; set; } = false;
    public string[] PathStartsWith { get; set; } = [];
    public string[] PathContains { get; set; } = [];
    public string[] SchemaAssemblyStartsWith { get; set; } = [];
    public string[] ExecludeSchemaAssemblyStartsWith { get; set; } = [];
}
