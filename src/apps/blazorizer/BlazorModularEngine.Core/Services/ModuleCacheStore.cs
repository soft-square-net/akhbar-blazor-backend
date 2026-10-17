using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BlazorModularEngine.Core.Common.Interfaces;
using BlazorModularEngine.Core.Models;

namespace BlazorModularEngine.Core.Services;

public class ModuleCacheStore
{
    private readonly Dictionary<string, ModuleMetadata> _metadataCache = new();
    private readonly Dictionary<string, Assembly> _assemblyCache = new();
    private readonly Dictionary<string, IBlazorModule> _instanceCache = new();

    public void StoreMetadata(IEnumerable<ModuleMetadata> metadataList)
    {
        foreach (var meta in metadataList) _metadataCache[meta.ModuleName] = meta;
    }

    public ModuleMetadata GetMetadata(string moduleName) => _metadataCache.TryGetValue(moduleName, out var meta) ? meta : null;
    public IEnumerable<ModuleMetadata> GetAllMetadata() => _metadataCache.Values;
    public void StoreAssembly(string moduleName, Assembly assembly) => _assemblyCache[moduleName] = assembly;
    public Assembly GetAssembly(string moduleName) => _assemblyCache.TryGetValue(moduleName, out var asm) ? asm : null;
    public void StoreInstance(string moduleName, IBlazorModule instance) => _instanceCache[moduleName] = instance;
    public IBlazorModule GetInstance(string moduleName) => _instanceCache.TryGetValue(moduleName, out var instance) ? instance : null;
    public bool HasInstance(string moduleName) => _instanceCache.ContainsKey(moduleName);
}
