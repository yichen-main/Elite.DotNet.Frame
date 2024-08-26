namespace Eywa.Domain.HumanResources.Endpoints.ManageOperates.AccountSignIn;
internal sealed class ClearAccountSignInVehicle : NodeEnlarge<ClearAccountSignInVehicle, NodeUnique>
{
    public override void Configure()
    {
        RequiredConfiguration();
    }
    public override Task HandleAsync(NodeUnique req, CancellationToken ct)
    {
        VerifyRootMember();
        var query = TableLayout.LetSelect<HumanUser>(nameof(HumanUser.Email), GetUserId());
        return ExecuteAsync(async (connection, readerAsync, transaction) =>
        {
            var user = await connection.QueryFirstOrDefaultAsync<HumanUser>(query, transaction).ConfigureAwait(false);
            if (user is not null)
            {
                var (sql, param) = TableLayout.LetInsert(new HumanUserRecord
                {
                    Id = TableLayout.GetSnowflakeId(),
                    UserId = user.Id,
                    LoginStatus = LoginStatus.SignOut,
                });
                await connection.ExecuteAsync(sql, param, transaction).ConfigureAwait(false);
            }
            await SendNoContentAsync(cancellation: ct).ConfigureAwait(false);
        }, ct: ct);
    }
}