namespace Eywa.Serve.Modularity.Commons.Structures.Protections;
internal static class InscribedExpand
{
    public static DomainType Archive(this DomainType type)
    {
        try
        {
            ReaderWriterLock.EnterWriteLock();
            DomainTypes.Add(type);
            return type;
        }
        finally
        {
            ReaderWriterLock.ExitWriteLock();
        }
    }
    public static bool Contains(in DomainType type)
    {
        try
        {
            ReaderWriterLock.EnterReadLock();
            return DomainTypes.Contains(type);
        }
        finally
        {
            ReaderWriterLock.ExitReadLock();
        }
    }
    public static ServiceCollection AddServices<T>(this ServiceCollection services) where T : BaseModule
    {
        var type = typeof(T);
        foreach (var validatorType in typeof(IValidator<>).GetSubInterfaces<T>())
        {
            var interfaceType = validatorType.GetInterfaces().First(x =>
            x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IValidator<>));
            services.AddTransient(interfaceType, validatorType);
        }
        services.AddFastEndpoints();
        services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining<T>());
        Modulars[type.Name] = type;
        return services;
    }
    public static void RemoveModule(in Type type) => Modulars.TryRemove(type.Name, out _);
    public static HashSet<DomainType> DomainTypes { get; } = [];
    public static ConcurrentDictionary<string, Type> Modulars { get; private set; } = [];
    static ReaderWriterLockSlim ReaderWriterLock { get; } = new();
}