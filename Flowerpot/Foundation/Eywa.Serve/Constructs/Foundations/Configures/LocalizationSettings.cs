namespace Eywa.Serve.Constructs.Foundations.Configures;
public sealed class LocalizationSettings
{
    public required string DefaultCulture { get; init; }
    public required string[] SupportedCultures { get; init; }
}