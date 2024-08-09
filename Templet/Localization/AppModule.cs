using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using NetLocalizer.Foundations;
using System.Globalization;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace NetLocalizer;

[DependsOn(typeof(AbpAutofacModule))]
public class AppModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // 註冊服務
        //context.Services.AddTransient<IMyService, MyService>();

        context.Services.AddSingleton<IStringLocalizerFactory>(new JsonStringLocalizerFactory("Resources"));

        //context.Services.AddControllersWithViews()
        //        .AddViewLocalization()
        //        .AddDataAnnotationsLocalization();

        context.Services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("fr-FR"),
                new CultureInfo("es-ES"),
                // 添加更多支持的語言
            };

            options.DefaultRequestCulture = new RequestCulture("en-US");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });
    }
}