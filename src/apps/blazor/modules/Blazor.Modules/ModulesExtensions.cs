using System.Reflection;
using System.Runtime.CompilerServices;
using FSH.Starter.Blazor.Modules.Configuration;
// using FSH.Starter.Blazor.Modules.Document.Blazor;
using FSH.Starter.Blazor.Shared;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using static FSH.Starter.Blazor.Modules.ModulesConstants;

namespace FSH.Starter.Blazor.Modules;
public static class ModulesExtensions
{
    public static string GetExecutingDirectorybyAppDomain()
    {
        string path = AppDomain.CurrentDomain.BaseDirectory;
        return path;
    }
    public static string GetAssemblyPathByCodeBase()
    {
        string codeBase = Assembly.GetExecutingAssembly().CodeBase;
        UriBuilder uri = new UriBuilder(codeBase);
        return Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
    }
    static Assembly LoadFromSameFolder(object sender, ResolveEventArgs args)
    {
        string folderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string assemblyPath = Path.Combine(folderPath, new AssemblyName(args.Name).Name + ".dll");
        if (!File.Exists(assemblyPath)) return null;
        Assembly assembly = Assembly.LoadFrom(assemblyPath);
        return assembly;
    }
    public static async  Task<IServiceCollection> ConfigureBlazorModules(this IServiceCollection services/*, LazyAssemblyLoader AssemblyLoader*/)
    {
        // AppDomain currentDomain = AppDomain.CurrentDomain;
        // currentDomain.AssemblyResolve += new ResolveEventHandler(LoadFromSameFolder);
        //var assemplyName = "FSH.Starter.Blazor.Modules.Document.Blazor";
        //var dynamicallyLoadedAssembly = Assembly.Load($"{assemplyName}.dll");
        //RegisteredModules.Add(assemplyName, dynamicallyLoadedAssembly);

        var loggerFactory = services.BuildServiceProvider().GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("BlazorModules");
        // Get the directory where the current assembly is located
        string executionPath = AppContext.BaseDirectory;

        // Find all DLL files in that directory
        string[] dllFiles = Directory.GetFiles(executionPath, "*.dll");
        string domainBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;

        var modulesAssembleyName = Assembly.GetExecutingAssembly().GetName().Name;
        var procPath = Environment.ProcessPath;
        // var dirName = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
        var codeBase = Assembly.GetCallingAssembly().Location;
        var entryAssembly = Assembly.GetEntryAssembly().Location;
        var baseDirectory = AppContext.BaseDirectory;  // GetExecutingDirectorybyAppDomain();
        string[] files = Directory.GetFiles(baseDirectory);
        string exePath = Environment.CurrentDirectory;
        var localPath = Path.GetDirectoryName(new Uri(baseDirectory).LocalPath);
        if (modulesAssembleyName != null)
        {
            var assemblyPath = Path.GetDirectoryName(baseDirectory); //typeof(ModulesExtensions).Assembly.Location;
            string[] filePaths = Directory.Exists(assemblyPath)? Directory.GetFiles(assemblyPath,$"{modulesAssembleyName}*.dll"): [];

            foreach (string filePath in filePaths)
            {
                string fileName = Path.GetFileName(filePath);
                if(fileName.StartsWith(modulesAssembleyName, StringComparison.OrdinalIgnoreCase) && fileName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                {
                    var modulesAssembly = Assembly.Load(fileName);
                    RegisteredModules.Add(modulesAssembleyName, modulesAssembly);
                    bool IsInitialized = await InitializeModule(modulesAssembly, logger);
                }
            }
            
        }


        //    // var mm = new DocumentModule(logger);
        //     GetAllModulesAssemblies().ToList().ForEach(async ans =>
        //     {
              
        //         var AssemblyName = ans.GetName().Name?.Replace(".", "", StringComparison.OrdinalIgnoreCase);
        //         if (!string.IsNullOrWhiteSpace(AssemblyName) && !RegisteredModules.ContainsKey(AssemblyName))
        //         {
        //             if (ans != null)
        //             {
        //                 bool IsInitialized = await InitializeModule(ans, logger);
        //                 // AppDomain.CurrentDomain.Load(ans.GetName());
        //                 // new DocumentModule(); // Force the static constructor to run
              
        //                 Console.WriteLine($"Blazor Module Loaded: {AssemblyName}, Assembly: {ans.FullName}");
        //                 Console.WriteLine($"Blazor Module Loaded: {AssemblyName}, Assembly: {ans.FullName}");
        //                 Console.WriteLine($"Blazor Module Loaded: {AssemblyName}, Assembly: {ans.FullName}");
        //                 Console.WriteLine($"Blazor Module Loaded: {AssemblyName}, Assembly: {ans.FullName}");
        //                 RegisteredModules.Add(AssemblyName, ans);
        //             }
        //         }
        //     }); 
        // var assemblies = await AssemblyLoader.LoadAssembliesAsync(RegisteredModules.Values.Select(a => a.FullName));
        services.AddSingleton<IModulesManager>(new ModulesManager(RegisteredModules));
        return services;
    }

    public static WebAssemblyHost UseBlazorModules(this WebAssemblyHost app)
    {
        ModulesConstants.RegisteredModules.ToList().ForEach(kv =>
        {
            var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("BlazorModules");
            logger.LogInformation($"Blazor Module Registered: {kv.Key}, Assembly: {kv.Value.FullName}");
            Console.WriteLine($"Blazor Module Registered: {kv.Key}, Assembly: {kv.Value.FullName}");
        });

        return app;
    }




    private static async Task<bool> InitializeModule(Assembly ans, ILogger logger)
    {
        string methodName = "InitializeAsync";
        // 1. Get the Type object for the specific class
        Type? type = ans.GetTypes()
            .Where(t => typeof(IBlazorModule).IsAssignableFrom(t))
            .FirstOrDefault();
        if (type == null) return false;
        // 3. Create an instance of the class
        object instance = Activator.CreateInstance(type, new object[] { logger });

        // 4. Get the MethodInfo object for the specific method
        // Use appropriate BindingFlags if the method is not public (e.g., BindingFlags.NonPublic | BindingFlags.Instance)
        MethodInfo methodInfo = type.GetMethod(methodName);

        if (methodInfo == null)
        {
            Console.WriteLine($"Error: Method '{methodName}' not found in the type.");
            return false;
        }

        // 5. Invoke the method
        // For non-static methods, the first parameter is the instance; for static methods, it is null
        object[] parameters = new object[] {  };
        object taskResult = methodInfo.Invoke(instance, parameters);
        // 4. Cast the result to a Task and await it
        if (taskResult is Task task)
        {
            await task; // Await the non-generic Task
            // If the method returns Task<T>, you need reflection to get the Result property
            if (task.GetType().IsGenericType && task.GetType().GetGenericTypeDefinition() == typeof(Task<>))
            {
                // The task is completed now, so accessing .Result is safe and won't deadlock
                object? resultValue = task.GetType().GetProperty("Result").GetValue(task, null);
                Console.WriteLine($"Invoked method returned: {resultValue}");
                return true;
            }
        }
        return false;
    }


    private static IEnumerable<Assembly> GetAllModulesAssemblies()
    {
        // Get all loaded assemblies in the current application domain
        // This includes the executing assembly and all referenced/linked assemblies that have been loaded
        return AppDomain.CurrentDomain.GetAssemblies()
            .Where(t => t.GetName().Name != null &&
                        t.GetName().Name!.StartsWith(ModulesGlobalNameSpace, StringComparison.OrdinalIgnoreCase)
                  );
    }
}
