namespace Elite.Core.Architects.Primaries.Enclosures;
internal static class InitializeExpand
{
    public static async Task<TPlatform> UseModularAsync<TFactory, TPlatform>(this IServiceCollection services) where TPlatform : notnull
    {
        var factoryType = typeof(TFactory);
        services.AddSingleton(factoryType);
        var provider = services.BuildServiceProvider();
        var type = factoryType.GetSpecificTypes<ModularAttribute>().First(x => factoryType.Name.IsFuzzy(x.Name));
        var info = type.GetMethod(type.GetCustomAttributes<ModularAttribute>().First().Name);
        var result = info?.Invoke(provider.GetService(type), parameters: default);
        if (result is Task task) await task.ConfigureAwait(false);
        return provider.GetService<TPlatform>()!;
    }
}