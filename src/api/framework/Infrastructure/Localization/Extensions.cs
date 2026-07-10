using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Shared.Enums;
using System.Globalization;

namespace FSH.Framework.Infrastructure.Localization;

public static class Extensions
{
    public static IServiceCollection AddJsonLocalization(this IServiceCollection services)
    {
        // إضافة خدمات الـ Localization الأساسية
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        return services;
    }

    public static IApplicationBuilder UseAppLocalization(this IApplicationBuilder app)
    {
        var localizationOptions = new RequestLocalizationOptions()
            .SetDefaultCulture(FSHLang.Values[0])
            .AddSupportedCultures(FSHLang.Values)
            .AddSupportedUICultures(FSHLang.Values);
        localizationOptions.RequestCultureProviders = new List<IRequestCultureProvider>
        {
            new QueryStringRequestCultureProvider(),
            new AcceptLanguageHeaderRequestCultureProvider() // الأفضل للـ APIs (Accept-Language: ar-EG)
        };


        app.UseRequestLocalization(localizationOptions);

        return app;
    }
}
