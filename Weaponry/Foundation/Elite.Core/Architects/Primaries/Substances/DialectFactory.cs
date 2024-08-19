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
                var cultureName = texts.RootElement.GetProperty(nameof(culture)).GetString();
                var @object = texts.RootElement.GetProperty(nameof(texts)).EnumerateObject();
                var contents = @object.ToDictionary(x => x.Name, x => x.Value.GetString(), StringComparer.Ordinal);
                if (cultureName is not null)
                {
                    if (LocalDialects.TryGetValue(cultureName, out var oldContents))
                    {
                        foreach (var (key, value) in contents) oldContents[key] = value;
                    }
                    else LocalDialects[cultureName] = contents;
                }
            }
        }
    }
    public IStringLocalizer Create(Type resourceSource) => new DialectLocalizer(LocalDialects);
    public IStringLocalizer Create(string baseName, string location) => new DialectLocalizer(LocalDialects);
    ConcurrentDictionary<string, Dictionary<string, string?>> LocalDialects { get; } = [];
}