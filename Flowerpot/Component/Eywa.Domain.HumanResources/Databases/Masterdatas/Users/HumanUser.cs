namespace Eywa.Domain.HumanResources.Databases.Masterdatas.Users;
internal sealed class HumanUser : NpgsqlBase
{
    public const string EmailIndex = $"{nameof(HumanUser)}{nameof(Email)}";
    public const string UserNoIndex = $"{nameof(HumanUser)}{nameof(UserNo)}";
    [RowInfo(UniqueIndex = true)] public required string Email { get; init; }
    [RowInfo(UniqueIndex = true)] public required string UserNo { get; init; }
    public required string UserName { get; init; }
    public required bool Disable { get; init; }
    public required string Salt { get; init; }
    public required string HashedText { get; init; }
    public static async IAsyncEnumerable<string> ClearAsync(NpgsqlConnection connection, string id)
    {
        var sql = TableLayout.LetSelect<HumanUser>(id);
        var user = await connection.QueryFirstOrDefaultAsync<HumanUser>(sql).ConfigureAwait(false);
        if (user is not null)
        {
            await foreach (var module in HumanUserModule.ClearRelyAsync(connection, user.Id).ConfigureAwait(false))
            {
                yield return module;
            }
            await foreach (var record in HumanUserRecord.ClearRelyAsync(connection, user.Id).ConfigureAwait(false))
            {
                yield return record;
            }
            yield return TableLayout.LetDelete<HumanUser>(id);
        }
    }
}