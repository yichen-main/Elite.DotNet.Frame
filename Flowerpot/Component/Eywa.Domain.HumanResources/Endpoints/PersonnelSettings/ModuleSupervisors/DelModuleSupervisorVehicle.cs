namespace Eywa.Domain.HumanResources.Endpoints.PersonnelSettings.ModuleSupervisors;
internal sealed class DelModuleSupervisorVehicle : NodeEnlarge<DelModuleSupervisorVehicle, NodeIdentify>
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
            await connection.ExecuteAsync(HumanUserModule.Clear(req.Id), transaction).ConfigureAwait(false);
            await SendNoContentAsync(cancellation: ct).ConfigureAwait(false);
        }, ct: ct);
    }
}