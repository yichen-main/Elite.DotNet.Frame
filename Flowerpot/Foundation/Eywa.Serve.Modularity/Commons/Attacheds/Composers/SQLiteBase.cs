namespace Eywa.Serve.Modularity.Commons.Attacheds.Composers;
public abstract class SQLiteBase
{
    public DateTime CreateTime { get; private set; } = DateTime.UtcNow;
    public string Id { get; init; } = IdAlgorithm.Next().ToString(CultureInfo.InvariantCulture);
}