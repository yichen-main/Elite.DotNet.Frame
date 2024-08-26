namespace Eywa.Serve.Modularity.Commons.Structures.Composers;
public abstract class NodeQueryer : NodeHeader
{
    short Page = 10;
    readonly short MaxSize = 10000;
    public string? Search { get; init; }
    public short PageSize
    {
        get => Page;
        set => Page = value > MaxSize ? MaxSize : value;
    }
    public short PageCount { get; init; } = 1;
}