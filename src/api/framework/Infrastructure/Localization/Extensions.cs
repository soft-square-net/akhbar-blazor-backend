//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Routing;
//using Microsoft.Extensions.DependencyInjection;
//using Shared.Enums;

//namespace FSH.Framework.Infrastructure.Localization;

//internal static class Extensions
//{
//    internal static IServiceCollection AddFSHLocalization(this IServiceCollection services)
//    {
//        ArgumentNullException.ThrowIfNull(services);
//        services.AddLocalization(options => options.ResourcesPath = "Resources");
//        services.AddRazorComponents().AddInteractiveServerComponents(); // or AddInteractiveWebAssemblyComponents

//        return services;
//    }

//    public static IApplicationBuilder UseFSHLocalization(this IApplicationBuilder app)
//    {
//        var localizationOptions = new RequestLocalizationOptions()
//            .SetDefaultCulture(FSHLang.Values[0])
//            .AddSupportedCultures(FSHLang.Values)
//            .AddSupportedUICultures(FSHLang.Values);

//        // Enable the localization middleware
//        app.UseRequestLocalization(localizationOptions);
//        return app;
//    }
//  }
