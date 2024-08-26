namespace Eywa.Serve.Modularity.Commons.Structures.Protections;
public static class BeforeExpand
{
    public static async ValueTask LeachAsync<T>(this IValidator<T> validator, T input)
    {
        var errors = (await validator.ValidateAsync(input).ConfigureAwait(false)).Errors;
        var error = string.Join(",\u00A0", errors.Select(x => x.ErrorMessage));
        if (!string.IsNullOrEmpty(error)) throw new Exception(error);
    }
    public static int ToYearMonthStamp(this short year, in short month) =>
        int.Parse(string.Create(CultureInfo.InvariantCulture, $"{year:D4}{month:D2}"), CultureInfo.InvariantCulture);
    public static (short year, short month) StampToYearMonth(this int date) => ((short)(date / 100), (short)(date % 100));
    public static string[] TrimText(this string letter, in char split = ',') => letter.TrimStart(split).TrimEnd(split).Split(split);
    public static IEnumerable<EnumCistern> ToMenu<T1, T2>(this IStringLocalizer<T1> localizer, string culture) where T2 : Enum
    {
        using (CultureHelper.Use(culture))
        {
            var type = typeof(T2);
            foreach (int enumNo in Enum.GetValues(type)) yield return new()
            {
                TypeNo = enumNo,
                TypeName = localizer[Enum.GetName(type, enumNo)!],
            };
        }
    }
    public static decimal ParseMoneyToDecimal(this string amount)
    {
        var numericPart = amount.Replace("$", string.Empty, StringComparison.Ordinal)
            .Replace("-", string.Empty, StringComparison.Ordinal);
        var result = Convert.ToDecimal(numericPart, CultureInfo.InvariantCulture);
        if (amount.StartsWith('-')) result = -result;
        return result;
    }
    public static string ParseDecimalToMoney(this decimal amount, in LanguageType languageType = LanguageType.English) =>
        amount.ToString("C", new CultureInfo(languageType.GetDesc()));
    public static double ParsePercentToDecimal(this string value) =>
        double.Parse(value.TrimEnd('%'), NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, CultureInfo.InvariantCulture);
    public static DateTime ParseDateTime(this string input, in string format = "yyyyMMddHHmmss") =>
        DateTime.TryParseExact(input, [format], CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate) ? parsedDate : default;
    public static IEnumerable<DomainType> GetModules() => InscribedExpand.DomainTypes;
    public static string ToLocalizer<T1, T2>(this IStringLocalizer<T1> localizer, in T2 type) where T2 : Enum
    {
        using (CultureHelper.Use(LanguageType.GetDesc())) return localizer[type.ToString()];
    }
    public static string LanguageTag(this int languageNo) => !Enum.IsDefined(typeof(LanguageType), languageNo)
        ? LanguageType.English.GetDesc() : ((LanguageType)Enum.ToObject(typeof(LanguageType), languageNo)).GetDesc();
    static LanguageType LanguageType { get; set; } = LanguageType.Traditional;
}