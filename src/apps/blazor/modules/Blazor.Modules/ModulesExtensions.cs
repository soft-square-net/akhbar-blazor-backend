using System.Reflection;
using FSH.Starter.Blazor.Modules.Configuration;
using FSH.Starter.BlazorShared.Services;
using FSH.Starter.BlazorShared.Services.Interfaces;
using FSH.Starter.BlazorShared;
using FSH.Starter.BlazorShared.Configurations;
using FSH.Starter.BlazorShared.Layout.Services;
using FSH.Starter.BlazorShared.Layout.Services.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using static FSH.Starter.Blazor.Modules.ModulesConstants;
using Microsoft.AspNetCore.Components;
using FSH.Starter.Blazor.Client.Layout;

namespace FSH.Starter.Blazor.Modules;
public static class ModulesExtensions
{
   public static async  Task<IServiceCollection> ConfigureBlazorModules(this IServiceCollection services, WebAssemblyHostBuilder builder/*, LazyAssemblyLoader AssemblyLoader*/)
    {
        await LoadModulesFromConfiguration(services, builder);

        // await new DocumentModule().InitializeAsync();
        services.AddSingleton<IModulesManager>(new ModulesManager(RegisteredModules));
        services.AddSingleton<IModulesLoader>(new ModulesLoader());
        services.AddScoped<IDynamicComponentService, DynamicComponentService>();
        services.AddScoped<ILayoutService, LayoutService>();
        services.AddSingleton<IAppNavigationService, BlazorAppNavigationService>();

        return services;
    }

    public  static async Task<WebAssemblyHost> UseBlazorModules(this WebAssemblyHost app)
    {
        var ModuleLoaderService = app.Services.GetService<IModulesLoader>();
        RegisteredModules.ToList().ForEach(kv =>
        {
            var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("BlazorModules");
            logger.LogInformation($"Blazor Module Registered: {kv.Key}, Assembly: {kv.Value.FullName}");
            Console.WriteLine($"Blazor Module Registered: {kv.Key}, Assembly: {kv.Value.FullName}");
        });

        List<Type> layoutTypes = new List<Type>();   
        ModulesCache.ToList().ForEach(async kv =>
        {
            ModuleLoaderService?.AddComponent(kv.Value.ModuleMenu);
            await kv.Value.UseModuleAsync(app);
            var modulesAssembly = kv.Value.GetType().Assembly;
            layoutTypes.AddRange(modulesAssembly.GetTypes()
                    .Where(t => t.IsAssignableTo(typeof(LayoutComponentBase)) && !t.IsAbstract));
        });


        // Setup Layouts Modules
        var layoutService = app.Services.GetRequiredService<ILayoutService>();
        SetupLayouts(layoutService, layoutTypes);
        return app;
    }

    private static async Task LoadModulesFromConfiguration(IServiceCollection services, WebAssemblyHostBuilder builder) {
        // AppDomain currentDomain = AppDomain.CurrentDomain;
        // currentDomain.AssemblyResolve += new ResolveEventHandler(LoadFromSameFolder);
        //var assemplyName = "FSH.Starter.Blazor.Modules.Document.Blazor";
        //var dynamicallyLoadedAssembly = Assembly.Load($"{assemplyName}.dll");
        //RegisteredModules.Add(assemplyName, dynamicallyLoadedAssembly);
        var serviceProvider = services.BuildServiceProvider();
        var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("BlazorModules");
        List<ModulesConfiguration> modules = builder.Configuration.GetSection("Modules").Get<List<ModulesConfiguration>>() ?? new List<ModulesConfiguration>();
        var modulesAssembleyName = Assembly.GetExecutingAssembly().GetName().Name;
        foreach (var module in modules.Where(m => m.IsEnabled))
        {
            try
            {
                var assemblyName = $"{modulesAssembleyName}.{module.Name}";
                // var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                // System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromAssemblyPath("");
                var modulesAssembly = Assembly.Load($"{assemblyName}.dll");
                RegisteredModules.Add(assemblyName, modulesAssembly);
                object? IsInitialized = await InitializeModule(modulesAssembly, services, builder, logger);
                // Get Layout Types from Assembly LayoutComponentBase
                
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error loading module assembly: {module.Name}");
            }
        }

    }

    private static async Task<object?> InitializeModule(Assembly ans, IServiceCollection services, WebAssemblyHostBuilder builder, ILogger logger)
    {
        Type? type = ans.GetTypes()
            .Where(t => typeof(IBlazorModule).IsAssignableFrom(t))
            .FirstOrDefault();
        if (type == null) return false;

        IBlazorModule instance = (IBlazorModule)Activator.CreateInstance(type, new object[] { logger });
        ModulesCache.Add(instance.Name, instance);
        await instance.InitializeAsync();
        await instance.ConfigureModule(services, builder);

        //object[] initParams = Array.Empty<object>();
        //_ = await RunInstanceMethodAsync(instance, type.GetMethod("InitializeAsync"), initParams);

        //object[] configParams = new object[] { services };
        //_ = await RunInstanceMethodAsync(instance, type.GetMethod("ConfigureModule"), configParams);

        return instance;
    }


    private static async Task SetupLayouts(ILayoutService layoutService, IEnumerable<Type>? layoutTypes)
    {
        
        foreach (var layoutType in layoutTypes)
        {
            var layoutInstance = (LayoutComponentBase)Activator.CreateInstance(layoutType);
            layoutService.RegisterLayout(layoutInstance);
            layoutService.SetLayout(layoutInstance);
            Console.WriteLine($"Layout Registered: {layoutType.FullName}");
        }
    }
}


#region UnusedMethods
//public static async Task<bool> RunInstanceMethodAsync(object? instance, MethodInfo? methodInfo,object[] parameters)
//{
//    if (instance == null || methodInfo == null)
//    {
//        Console.WriteLine($"Error: Method not found in the function RunInstanceMethodAsync.");
//        return false;
//    }
//    // string methodName = methodInfo.Name;
//    object taskResult = methodInfo.Invoke(instance, parameters);
//    if (taskResult is Task task)
//    {
//        await task; 
//        if (task.GetType().IsGenericType && task.GetType().GetGenericTypeDefinition() == typeof(Task<>))
//        {
//            object? resultValue = task.GetType().GetProperty("Result").GetValue(task, null);
//            Console.WriteLine($"Invoked method returned: {resultValue}");
//            return true;
//        }
//    }
//    return false;
//}


//private static async Task LoadModulesFromConfiguration1(ILogger logger)
//{
//    foreach (var ans in System.Runtime.Loader.AssemblyLoadContext.Default.Assemblies)
//    {
//        var AssemblyName = ans.GetName().Name?.Replace(".", "", StringComparison.OrdinalIgnoreCase);
//        if (!string.IsNullOrWhiteSpace(AssemblyName) && !RegisteredModules.ContainsKey(AssemblyName))
//        {
//            if (ans != null)
//            {
//                bool IsInitialized = await InitializeModule(ans, logger);
//                // AppDomain.CurrentDomain.Load(ans.GetName());
//                // new DocumentModule(); // Force the static constructor to run
//                Console.WriteLine($"Blazor Module Loaded: {AssemblyName}, Assembly: {ans.FullName}");
//                RegisteredModules.Add(AssemblyName, ans);
//            }
//        }
//    }
//}



//private static IEnumerable<Assembly> GetAllModulesAssemblies()
//{
//    // Get all loaded assemblies in the current application domain
//    // This includes the executing assembly and all referenced/linked assemblies that have been loaded
//    return AppDomain.CurrentDomain.GetAssemblies()
//        .Where(t => t.GetName().Name != null &&
//                    t.GetName().Name!.StartsWith(ModulesGlobalNameSpace, StringComparison.OrdinalIgnoreCase)
//              );
//}



//var procPath = Environment.ProcessPath;
//// var dirName = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
//var codeBase = Assembly.GetCallingAssembly().Location;
//var entryAssembly = Assembly.GetEntryAssembly().Location;
// // Get the directory where the current assembly is located
//     string executionPath = AppContext.BaseDirectory;
// // Find all DLL files in that directory
// string[] dllFiles = Directory.GetFiles(executionPath, "*.dll");
// string domainBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
// string assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
// 
// var modulesAssembleyName = Assembly.GetExecutingAssembly().GetName().Name;
// 
// var baseDirectory = AppContext.BaseDirectory;  // GetExecutingDirectorybyAppDomain();
// string[] files = Directory.GetFiles(baseDirectory);
// string exePath = Environment.CurrentDirectory;
// var localPath = Path.GetDirectoryName(new Uri(baseDirectory).LocalPath);


//  if (modulesAssembleyName != null)
//      {
//          var assemblyPath = Path.GetDirectoryName(baseDirectory); //typeof(ModulesExtensions).Assembly.Location;
//          string[] filePaths = Directory.Exists(assemblyPath) ? Directory.GetFiles(assemblyPath, $"{modulesAssembleyName}*.dll") : [];
//  
//          foreach (string filePath in filePaths)
//          {
//              string fileName = Path.GetFileName(filePath);
//              if(fileName.StartsWith(modulesAssembleyName, StringComparison.OrdinalIgnoreCase) && fileName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
//              {
//                  var modulesAssembly = Assembly.Load(fileName);
//                  RegisteredModules.Add(modulesAssembleyName, modulesAssembly);
//                  bool IsInitialized = await InitializeModule(modulesAssembly, logger);
//              }
//          }
//          
//      }


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

// public static string GetExecutingDirectorybyAppDomain()
// {
//     string path = AppDomain.CurrentDomain.BaseDirectory;
//     return path;
// }
// public static string GetAssemblyPathByCodeBase()
// {
//     string codeBase = Assembly.GetExecutingAssembly().CodeBase;
//     UriBuilder uri = new UriBuilder(codeBase);
//     return Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
// }
// static Assembly LoadFromSameFolder(object sender, ResolveEventArgs args)
// {
//     string folderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
//     string assemblyPath = Path.Combine(folderPath, new AssemblyName(args.Name).Name + ".dll");
//     if (!File.Exists(assemblyPath)) return null;
//     Assembly assembly = Assembly.LoadFrom(assemblyPath);
//     return assembly;
// }
#endregion
