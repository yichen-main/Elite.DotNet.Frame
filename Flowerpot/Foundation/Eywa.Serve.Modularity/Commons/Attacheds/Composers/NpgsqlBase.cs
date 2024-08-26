namespace Eywa.Serve.Modularity.Commons.Attacheds.Composers;
public abstract class NpgsqlBase
{
    public DateTime CreateTime { get; private set; } = DateTime.UtcNow;
    public string Id { get; init; } = null!;
}