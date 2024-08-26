namespace Eywa.Domain.HumanResources.Endpoints.ManageOperates.AccountRegisters;
internal sealed class DelAccountRegisterVehicle : NodeEnlarge<DelAccountRegisterVehicle, NodeIdentify>
{
    public override void Configure()
    {
        RequiredConfiguration();
    }
    public override Task HandleAsync(NodeIdentify req, CancellationToken ct)
    {
        VerifyRootMember();
        return ExecuteAsync(async (connection, readerAsync, transaction) =>
        {
            await foreach (var sql in HumanUser.ClearAsync(connection, req.Id).ConfigureAwait(false).WithCancellation(ct))
            {
                await connection.ExecuteAsync(sql, transaction).ConfigureAwait(false);
            }
            await SendNoContentAsync(cancellation: ct).ConfigureAwait(false);
        }, ct: ct);
    }
}