namespace Eywa.Domain.HumanResources.Endpoints.PersonnelSettings.ModuleMembers;
internal sealed class DelModuleMemberVehicle() : NodeEnlarge<DelModuleMemberVehicle, NodeIdentify>(RolePolicy.Owner)
{
    public override void Configure()
    {
        RequiredConfiguration();
    }
    public override Task HandleAsync(NodeIdentify req, CancellationToken ct)
    {
        return ExecuteAsync(async (connection, readerAsync, transaction) =>
        {
            await VerifyHumanMemberAsync().ConfigureAwait(false);
            await connection.ExecuteAsync(HumanUserModule.Clear(req.Id), transaction).ConfigureAwait(false);
            await SendNoContentAsync(cancellation: ct).ConfigureAwait(false);
        }, ct: ct);
    }
}