namespace Eywa.Serve.Constructs.Grindstones.Substances;
public sealed class ModuleProfile
{
    public TextOwnInformation OwnInformation { get; init; } = new();
    public TextMiddlePlatform MiddlePlatform { get; init; } = new();
    public TextHTTPAuthentication HTTPAuthentication { get; init; } = new();
    public sealed class TextOwnInformation
    {
        public string SerialNo { get; init; } = $"{nameof(Eywa)}{nameof(Serve)}-{Guid.NewGuid().ToString("N")[..5]}";
    }
    public sealed class TextMiddlePlatform
    {
        public bool Enabled { get; set; }
        public string WSDL { get; init; } = "iiot/service.asmx".ToUrlAddress(7260);
    }
    public sealed class TextHTTPAuthentication
    {
        public string Secret { get; init; } = ProfileExpand.RandomInteger64Bits().ToMD5();
        public string AccountName { get; init; } = "reformtek";
        public int ExpirySeconds { get; init; } = 300;
        public int ExpirationDays { get; init; } = 7;
    }
}