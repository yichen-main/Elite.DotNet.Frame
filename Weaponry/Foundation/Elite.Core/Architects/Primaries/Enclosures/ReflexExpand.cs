namespace Elite.Core.Architects.Primaries.Enclosures;
public static class ReflexExpand
{
    public static Type[] GetAssemblyTypes(this Type type)
    {
        var assembly = Assembly.GetAssembly(type);
        return assembly is null ? [] : assembly.GetTypes();
    }
    public static IEnumerable<Type> GetSpecificTypes<T>(this Type type) where T : Attribute => type.Assembly.GetSpecificTypes<T>();
    public static IEnumerable<Type> GetSpecificTypes<T>(this Type[] types) where T : Attribute => types.Where(x => x.GetCustomAttributes<T>().Any()) ?? [];
    public static IEnumerable<Type> GetSpecificTypes<T>(this Assembly assembly) where T : Attribute => assembly.GetTypes().GetSpecificTypes<T>();
    public static IEnumerable<Stream> GetDialectResourceStreams(this Assembly assembly)
    {
        foreach (var name in assembly.GetManifestResourceNames().Where(x => x.EndsWith(FileExtension.Jaon, StringComparison.OrdinalIgnoreCase)))
        {
            var stream = assembly.GetManifestResourceStream(name);
            if (stream is not null) yield return stream;
        }
    }
}