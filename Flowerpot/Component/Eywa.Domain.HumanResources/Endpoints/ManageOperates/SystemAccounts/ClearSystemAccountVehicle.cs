namespace Eywa.Domain.HumanResources.Endpoints.ManageOperates.SystemAccounts;
internal sealed class ClearSystemAccountVehicle : NodeEnlarge<ClearSystemAccountVehicle, NodeUnique>
{
    public override void Configure()
    {
        RequiredConfiguration();
    }
    public override Task HandleAsync(NodeUnique req, CancellationToken ct)
    {
        VerifyRootMember();
        if (File.Exists(CiphertextPolicy.RootCodePath)) File.Delete(CiphertextPolicy.RootCodePath);
        return SendNoContentAsync(cancellation: ct);
    }
}