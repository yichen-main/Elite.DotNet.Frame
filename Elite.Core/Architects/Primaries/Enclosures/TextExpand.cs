namespace Elite.Core.Architects.Primaries.Enclosures;
public static class TextExpand
{
    public static bool IsFuzzy(this string input, in string pattern) => new Regex(pattern, RegexOptions.None, TimeSpan.FromSeconds(1)).IsMatch(input);
}