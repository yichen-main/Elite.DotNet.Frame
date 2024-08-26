namespace Eywa.Serve.Constructs.Grindstones.Inventories;
public readonly struct OperationInfo
{
    public required int Port { get; init; }
    public required string Name { get; init; }
    public required ListenOptions ListenOptions { get; init; }
}