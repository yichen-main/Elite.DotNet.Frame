namespace Elite.Core.Architects.Primaries.Enclosures;
public static class TextExpand
{
    public static bool IsFuzzy(this string input, in string pattern) => new Regex(pattern, RegexOptions.None, TimeSpan.FromSeconds(1)).IsMatch(input);
    public static bool IsMatch(this string? str1, in string? str2, in StringComparison type = StringComparison.Ordinal) => string.Equals(str1, str2, type);
}