using Microsoft.Extensions.Localization;

namespace NetLocalizer.Foundations;

public class JsonStringLocalizerFactory(string resourcesPath) : IStringLocalizerFactory
{
    public IStringLocalizer Create(Type resourceSource)
    {
        var typeName = resourceSource.FullName;
        var namespacePath = typeName.Replace('.', Path.DirectorySeparatorChar);
        var baseName = Path.Combine(resourcesPath, namespacePath);
        return new JsonStringLocalizer(baseName, resourcesPath);
    }

    public IStringLocalizer Create(string baseName, string location)
    {
        return new JsonStringLocalizer(baseName, location);
    }
}