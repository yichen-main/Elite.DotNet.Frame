namespace Eywa.Domain.HumanResources.Databases.Masterdatas.Users;
internal sealed class HumanUserModule : NpgsqlBase
{
    [RowInfo(ForeignKey = true)] public required string UserId { get; init; }
    public required DomainType DomainType { get; init; }
    public required RolePolicy RolePolicy { get; init; }
    public required bool Disable { get; init; }
    public required string Modifier { get; init; }
    public required DateTime ModifyTime { get; init; }
    public required string Creator { get; init; }
    public static string Clear(string id) => TableLayout.LetDelete<HumanUserModule>(id);
    public static async IAsyncEnumerable<string> ClearRelyAsync(NpgsqlConnection connection, string userId)
    {
        var sql = TableLayout.LetSelect<HumanUserModule>(nameof(UserId), userId);
        var modules = await connection.QueryAsync<HumanUserModule>(sql).ConfigureAwait(false);
        foreach (var module in modules.OrEmptyIfNull())
        {
            yield return Clear(module.Id);
        }
    }
}