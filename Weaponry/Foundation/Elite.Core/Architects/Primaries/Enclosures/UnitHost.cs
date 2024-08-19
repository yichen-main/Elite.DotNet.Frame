namespace Elite.Core.Architects.Primaries.Enclosures;
public static class UnitHost
{
    public static async Task CreateAsync<T>(Action<IServiceCollection>? services = default, Action<Exception>? e = default) where T : notnull
    {
        try
        {
            var builder = WebApplication.CreateBuilder();
            var assembly = Assembly.GetAssembly(typeof(T));

            builder.Services.AddControllers();

            builder.Services.AddSingleton<IStringLocalizerFactory>(x => new DialectFactory(assembly));
            builder.Services.AddLocalization();

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>((context, containerBuilder) =>
            {
                ModuleLoader moduleLoader = new(containerBuilder);
                moduleLoader.LoadModules(assembly);
            });

            if (services is not null) services(builder.Services);
            var app = builder.Build();

            app.UseRouting();
            app.MapControllers();

            var supportedCultures = new[]
            {
                new CultureInfo("zh-CN"),
                new CultureInfo("en-US"),
                new CultureInfo("zh-TW"),
            };

            var defaultCulture = "en-US";

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                ApplyCurrentCultureToResponseHeaders = true,
                DefaultRequestCulture = new(defaultCulture),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
                RequestCultureProviders =
                [
                    new DialectProvider(defaultCulture, supportedCultures),
                    new AcceptLanguageHeaderRequestCultureProvider(),
                ],
            });
            await app.RunAsync().ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            if (e is not null) e(exception);
        }
    }
}