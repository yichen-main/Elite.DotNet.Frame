namespace Eywa.Domain.HumanResources.Databases.Masterdatas.Users;
internal sealed class HumanUserPosition : NpgsqlBase
{
    public const string PositionNoIndex = $"{nameof(HumanUserPosition)}{nameof(PositionNo)}";
    [RowInfo(UniqueIndex = true)] public required string PositionNo { get; init; }
    public required string PositionName { get; init; }
    public required string Modifier { get; init; }
    public required DateTime ModifyTime { get; init; }
    public required string Creator { get; init; }
}