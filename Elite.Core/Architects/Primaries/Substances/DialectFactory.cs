namespace Elite.Core.Architects.Primaries.Substances;
internal sealed class DialectFactory : IStringLocalizerFactory
{
    public DialectFactory(Assembly? assembly)
    {
        if (assembly is not null)
        {
            foreach (var culture in assembly.GetDialectResourceStreams() ?? [])
            {
                using StreamReader reader = new(culture);
                var texts = JsonDocument.Parse(reader.ReadToEnd());
                var name = texts.RootElement.GetProperty(nameof(culture)).GetString();
                var enumerator = texts.RootElement.GetProperty(nameof(texts)).EnumerateObject();
                var contents = enumerator.ToDictionary(x => x.Name, x => x.Value.GetString());
                if (name is not null) LocalDialects.AddOrUpdate(name, contents, (_, oldValue) =>
                {
                    foreach (var (key, value) in contents) oldValue[key] = value;
                    return oldValue;
                });
            }
        }
    }
    public IStringLocalizer Create(Type resourceSource) => new DialectLocalizer(LocalDialects);
    public IStringLocalizer Create(string baseName, string location) => new DialectLocalizer(LocalDialects);
    ConcurrentDictionary<string, Dictionary<string, string?>> LocalDialects { get; } = [];
}