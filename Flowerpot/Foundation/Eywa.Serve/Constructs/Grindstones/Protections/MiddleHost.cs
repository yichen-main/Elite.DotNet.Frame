namespace Eywa.Serve.Constructs.Grindstones.Protections;
public static class MiddleHost
{
    public static WebApplicationBuilder Create(params string[]? srgs) => WebApplication.CreateBuilder(new WebApplicationOptions
    {
        Args = srgs,
        ContentRootPath = AppContext.BaseDirectory,
    });
    public static ValueTask CreateAsync<T>(Action<Exception>? e = default) where T : BattleshipBuilder
    {
        return CreateAsync<StandardPlatform, T>(e: exception =>
        {
            if (e is not null) e(exception);
        });
    }
    public static async ValueTask CreateAsync<T, TFactory>(Action<Exception>? e = default)
        where TFactory : BattleshipBuilder where T : PlatformBuilder
    {
        try
        {
            var initialAsync = InternalExpand.InitialAsync<TFactory>();
            var service = new ServiceCollection().AddSingleton<T>().AddCharger();
            var platform = await service.CreateAsync<TFactory, T>().ConfigureAwait(false);
            if (platform.WebApp is not null)
            {
                await initialAsync.ConfigureAwait(false);
                await platform.WebApp.RunAsync().ConfigureAwait(false);
            }
        }
        catch (Exception exception)
        {
            if (e is not null) e(exception);
            await FileLayout.OutputAsync(new()
            {
                Type = typeof(TFactory).GetType(),
                Line = exception.GetLine(),
                Name = nameof(CreateAsync),
                Message = exception.Message,
            }).ConfigureAwait(false);
        }
    }
    public static async Task<T> CreateAsync<TFactory, T>(this IServiceCollection services) where T : class
    {
        var factory = typeof(TFactory);
        services.AddSingleton(factory);
        var provider = services.BuildServiceProvider();
        var type = factory.GetSpecificTypes<ModularAttribute>().First(x => factory.Name.IsFuzzy(x.Name));
        var info = type.GetMethod(type.GetCustomAttributes<ModularAttribute>().First().Name);
        var result = info?.Invoke(provider.GetService(type), parameters: default);
        if (result is Task task) await task.ConfigureAwait(false);
        return provider.GetService<T>()!;
    }
}