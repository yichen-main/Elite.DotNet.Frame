using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Text.Json;

namespace NetLocalizer.Foundations;
public class JsonStringLocalizer : IStringLocalizer
{
    private readonly Dictionary<string, string> _localizedStrings;
    public JsonStringLocalizer(string baseName, string location)
    {
        var culture = CultureInfo.CurrentUICulture.Name;
        var filePath = Path.Combine(location, $"{baseName}.{culture}.json");

        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            var localizationData = JsonSerializer.Deserialize<LocalizationData>(json);
            _localizedStrings = localizationData.Texts;
        }
        else
        {
            _localizedStrings = new Dictionary<string, string>();
        }
    }
    public LocalizedString this[string name]
    {
        get
        {
            if (_localizedStrings.TryGetValue(name, out var value))
            {
                return new LocalizedString(name, value);
            }

            return new LocalizedString(name, name, resourceNotFound: true);
        }
    }
    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            if (_localizedStrings.TryGetValue(name, out var value))
            {
                return new LocalizedString(name, string.Format(value, arguments));
            }

            return new LocalizedString(name, name, resourceNotFound: true);
        }
    }
    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        foreach (var item in _localizedStrings)
        {
            yield return new LocalizedString(item.Key, item.Value);
        }
    }
}
public class LocalizationData
{
    public string Culture { get; set; }
    public Dictionary<string, string> Texts { get; set; }
}