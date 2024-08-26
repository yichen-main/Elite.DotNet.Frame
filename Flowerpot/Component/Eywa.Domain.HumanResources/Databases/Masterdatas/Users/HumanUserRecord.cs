namespace Eywa.Domain.HumanResources.Databases.Masterdatas.Users;
internal sealed class HumanUserRecord : NpgsqlBase
{
    [RowInfo(ForeignKey = true)] public required string UserId { get; init; }
    public required LoginStatus LoginStatus { get; init; }
    public static async IAsyncEnumerable<string> ClearRelyAsync(NpgsqlConnection connection, string userId)
    {
        var sql = TableLayout.LetSelect<HumanUserRecord>(nameof(UserId), userId);
        var records = await connection.QueryAsync<HumanUserRecord>(sql).ConfigureAwait(false);
        foreach (var record in records.OrEmptyIfNull())
        {
            yield return TableLayout.LetDelete<HumanUserRecord>(record.Id);
        }
    }
}