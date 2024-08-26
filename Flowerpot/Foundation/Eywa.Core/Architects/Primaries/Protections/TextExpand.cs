namespace Eywa.Core.Architects.Primaries.Protections;
public static class TextExpand
{
    public const char Link = '@';
    public const char Empty = '\u00A0';
    public const string EnableTag = "1";
    public const string DisableTag = "0";
    public const string DefaultDateTime = "yyyy/MM/dd HH:mm:ss";
    public static string LetConvertPath(this string input) => input.ToSnakeCase('-');
    public static string ToSnakeCase(this string input, in char delimiter = '_')
    {
        StringBuilder result = new();
        if (string.IsNullOrEmpty(input)) return input;
        result.Append(char.ToLowerInvariant(input[default]));
        for (int i = 1; i < input.Length; i++)
        {
            if (char.IsUpper(input[i]))
            {
                result.Append(delimiter);
                result.Append(char.ToLowerInvariant(input[i]));
            }
            else result.Append(input[i]);
        }
        return result.ToString();
    }
}