namespace Eywa.Serve.Constructs.Grindstones.Quarterlies;
public interface IConfigurePicker
{
    void InitialProfile<T>() where T : class;
    string? GetAppSettings(in string key);
    T? GetAppSettings<T>() where T : class;
    IEnumerable<IFileInfo> GetEywaLinkXmlFile(in string directoryName);
    IEnumerable<IFileInfo> GetEywaLinkCsvFile(in string directoryName);
    ValueTask<TContent> GetProfileAsync<T, TContent>() where T : class;
    Task GetConfigurationAsync<T>(bool cover = default, CancellationToken ct = default) where T : class;
    ValueTask SetEywaLinkXmlAsync<T>(in string directoryName, in string fileName, in T entity) where T : class;
    ValueTask SetProfileAsync<T, TContent>(in TContent content, in CancellationToken ct = default) where T : class;
    ModuleProfile ModuleProfile { get; }
}

[Dependent(ServiceLifetime.Singleton)]
file sealed class ConfigurePicker(IConfiguration configuration) : IConfigurePicker
{
    public void InitialProfile<T>() where T : class => GetConfigurationAsync<T>(cover: true).Wait();
    public string? GetAppSettings(in string key) => configuration.GetSection(key).Value;
    public T? GetAppSettings<T>() where T : class => configuration.GetSection(typeof(T).Name).Get<T>();
    public IEnumerable<IFileInfo> GetEywaLinkXmlFile(in string directoryName) =>
        EywaLinkProvider.GetDirectoryContents(directoryName).Where(x => FileLayout.IsXmlFile(x.PhysicalPath));
    public IEnumerable<IFileInfo> GetEywaLinkCsvFile(in string directoryName) =>
        EywaLinkProvider.GetDirectoryContents(directoryName).Where(x => FileLayout.IsCsvFile(x.PhysicalPath));
    public ValueTask<TContent> GetProfileAsync<T, TContent>() where T : class => FileLayout.ReadProfileAsync<T, TContent>();
    public async Task GetConfigurationAsync<T>(bool cover = default, CancellationToken ct = default) where T : class
    {
        ModuleProfile = await GetProfileAsync<T, ModuleProfile>().ConfigureAwait(false);
        if (cover) await SetProfileAsync<T, ModuleProfile>(ModuleProfile, ct).ConfigureAwait(false);
    }
    public ValueTask SetEywaLinkXmlAsync<T>(in string directoryName, in string fileName, in T entity)
        where T : class => FileLayout.WriteXmlFileAsync(directoryName, entity, fileName);
    public ValueTask SetProfileAsync<T, TContent>(in TContent content, in CancellationToken ct = default)
        where T : class => FileLayout.WriteProfileAsync<T, TContent>(content, ct);
    public ModuleProfile ModuleProfile { get; private set; } = null!;
    static PhysicalFileProvider EywaLinkProvider => new(FileLayout.EywaLinkFolderLocation);
}