using static Eywa.Core.Architects.Primaries.Protections.ArchiveExpand;

namespace Eywa.Core.Architects.Primaries.Inventories;
public readonly ref struct FileLayout
{
    public static bool IsCsvFile(in string? filePath) => filePath.ExistFile(Extension.CSV);
    public static bool IsXmlFile(in string? filePath) => filePath.ExistFile(Extension.XML);
    public static void ClearEmptyFolders(in string path) => path.ToDeleteEmptyFolder();
    public static string GetProjectName<T>() => GetProjectName(typeof(T));
    public static string GetProjectName(in Type type) => type.Assembly.GetName().Name!;
    public static string GetProjectName<T>(in T entity) where T : notnull => GetProjectName(entity.GetType());
    public static string GetRootFolderPath(in string directoryName) => directoryName.GetFolderPath();
    public static string GetLogFolderPath(in string directoryName) => directoryName.GetFolderPath(Folder.Log);
    public static string GetConfigFolderPath(in string directoryName) => directoryName.GetFolderPath(Folder.Config);
    public static string GetModuleFilePath(in string fileName) => Path.Combine(ModuleFolderLocation, fileName);
    public static T? GetRootJsonFile<T>(string fileName = "appsettings.json") => ReadJsonFile<T>(RootFolderLocation, fileName);
    public static ValueTask<TContent> ReadProfileAsync<T, TContent>() where T : class
    {
        return ConfigFolderLocation.ReadYamlFileAsync<TContent>(GetProjectName<T>());
    }
    public static T? ReadJsonFile<T>(in string filePath, in string fileName) => filePath.ReadJsonFile<T>(fileName);
    public static T? ReadXmlFile<T>(in string filePath, in string fileName) => filePath.ReadXmlFile<T>(fileName);
    public static ValueTask WriteProfileAsync<T, TContent>(TContent content, CancellationToken ct = default) where T : class
    {
        return ConfigFolderLocation.WriteYamlFileAsync(GetProjectName<T>(), content, ct);
    }
    public static async ValueTask WriteXmlFileAsync<T>(string directoryName, T content, string fileName = nameof(File))
    {
        var filePath = Path.Combine(directoryName.GetFolderPath(Folder.EywaLink), $"{fileName}{Extension.XML}");
        if (!File.Exists(filePath))
        {
            StreamWriter writer = new(filePath);
            await using (writer.ConfigureAwait(false))
            {
                new XmlSerializer(typeof(T), string.Empty).Serialize(XmlWriter.Create(writer, new()
                {
                    Indent = true,
                    Encoding = Encoding.UTF8,
                    OmitXmlDeclaration = true,
                }), content, new([new XmlQualifiedName(string.Empty, string.Empty)]));
            }
        }
    }
    public static Task LogAsync(in object value) => RecordAsync(nameof(System), Regex.Unescape(value.ToJson()));
    public static ValueTask OutputAsync(in HistoryLetter historyLetter) => HistoryTransfer.Writer.WriteAsync(historyLetter);
    public static string LogFolderLocation => Folder.Log.GetFolderPath();
    public static string RootFolderLocation => Folder.Root.GetFolderPath();
    public static string ModuleFolderLocation => Folder.Module.GetFolderPath();
    public static string ConfigFolderLocation => Folder.Config.GetFolderPath();
    public static string EywaLinkFolderLocation => Folder.EywaLink.GetFolderPath();
}