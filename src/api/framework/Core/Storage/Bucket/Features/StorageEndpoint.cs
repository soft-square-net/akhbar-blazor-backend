using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.Framework.Core.Storage.Bucket.Features;

public class StorageEndpoint
{
    private StorageEndpoint(string systemName, string displayName)
    {
        SystemName = systemName;
        DisplayName = displayName;
    }
    public string SystemName { get; set; }
    public string DisplayName { get; set; }

    public static StorageEndpoint Create(string systemName, string displayName)
    { 
        return new StorageEndpoint(systemName, displayName);
    }
}
