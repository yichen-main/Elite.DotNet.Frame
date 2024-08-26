namespace Eywa.Serve.Constructs.Foundations.Configures;
public sealed class MessageManager
{
    public required TextOpenTelemetry OpenTelemetry { get; init; }
    public required TextRabbitMQ RabbitMQ { get; init; }
    public readonly struct TextOpenTelemetry
    {
        public required string Endpoint { get; init; }
        public required string Headers { get; init; }
    }
    public readonly struct TextRabbitMQ
    {
        public required string Username { get; init; }
        public required string Password { get; init; }
    }
}