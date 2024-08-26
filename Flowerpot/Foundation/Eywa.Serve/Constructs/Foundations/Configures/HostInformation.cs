namespace Eywa.Serve.Constructs.Foundations.Configures;
public sealed class HostInformation
{
    public required string Identifier { get; init; }
    public required TextHTTP HTTP { get; init; }
    public required TextHTTPS HTTPS { get; init; }
    public required TextMqttServer MqttServer { get; init; }
    public required TextOpcUaServer OpcUaServer { get; init; }
    public required TextInfluxDB InfluxDB { get; init; }

    [StructLayout(LayoutKind.Auto)]
    public readonly struct TextHTTP
    {
        public required int Port { get; init; }
        public required TextPath Path { get; init; }
        public readonly struct TextPath
        {
            public required string SOAP { get; init; }
            public required string GraphQL { get; init; }
        }
    }

    [StructLayout(LayoutKind.Auto)]
    public readonly struct TextHTTPS
    {
        public required int Port { get; init; }
        public required bool Enabled { get; init; }
        public required TextCertificate Certificate { get; init; }
        public readonly struct TextCertificate
        {
            public required string Location { get; init; }
            public required string Password { get; init; }
        }
    }
    public readonly struct TextMqttServer
    {
        public required int Port { get; init; }
        public required string Name { get; init; }
        public required string Path { get; init; }
    }
    public readonly struct TextOpcUaServer
    {
        public required int Port { get; init; }
        public required string Name { get; init; }
        public required string Path { get; init; }
    }
    public readonly struct TextInfluxDB
    {
        public required string Url { get; init; }
        public required string Token { get; init; }
        public required string OrganizeName { get; init; }
        public required int RetentionDay { get; init; }
        public required int TimeoutSeconds { get; init; }
    }
}