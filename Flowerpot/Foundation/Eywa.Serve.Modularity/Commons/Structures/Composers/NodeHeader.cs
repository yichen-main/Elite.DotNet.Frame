namespace Eywa.Serve.Modularity.Commons.Structures.Composers;
public abstract class NodeHeader
{
    [FromHeader(HeaderName.AcceptTimeZone)] public required int TimeZone { get; init; }
    [FromHeader(HeaderName.AcceptTimeFormat)] public required string TimeFormat { get; init; }
}