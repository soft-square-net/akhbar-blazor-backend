using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.Starter.ElsaWorkflow.Infrastructure.Auth.ApiKey;

public class ApiKey
{
    public string Key { get; set; }
    public string Owner { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Expires { get; set; }
    public List<string> Roles { get; set; } = new();
    public bool IsActive { get; set; } = true;
}
